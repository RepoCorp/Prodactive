using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Util;
using Android.Widget;
using ProdactiveMovil.Database;
using ProdactiveMovil.Helpers;
using ProdactiveMovil.ModelServicio;
using ProdactiveMovil.ModelServicio.SQLite;
using ProdactiveMovil.Services;
using ServiceStack;
using ServiceStack.Text;
using SQLite.Net.Platform.XamarinAndroid;

namespace ProdactiveMovil
{
    [Activity(Label = "RetoActivity")]
    public class RetoActivity : Activity
    {

        object isSendingReporte = new object();
        private bool isSending  = false;
        int count = 1;
        private Intent service;

        private StepServiceBinder       binder;
        private StepServiceConnection   serviceConnection;
        
        private bool firstRun = true;
        private bool registered;
        
        private Handler handler;

        public bool              IsBound
        {
            get; 
            set;
        }
        public StepServiceBinder Binder 
        {
            get { return binder; }
            set
            {
                binder = value;
                if (binder == null)
                    return;

                HandlePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("StepsToday"));

                if (registered)
                    binder.StepService.PropertyChanged -= HandlePropertyChanged;

                binder.StepService.PropertyChanged += HandlePropertyChanged;
                registered = true;
            }



        }

        private TextView tv;
        private TextView tcalorias;

        private double StepLength = 0;
        private double BodyWeight = 0;

        private Int64 pasosUltimaActualizacion = 0;
        private Int64 contPasos=0;

        private System.Timers.Timer Reporte;

        private LoginResponse lr;
        private bool swButton = false;
        #region calorias

        private static double METRIC_RUNNING_FACTOR = 1.02784823;
        private static double IMPERIAL_RUNNING_FACTOR = 0.75031498;

        private static double METRIC_WALKING_FACTOR = 0.708;
        private static double IMPERIAL_WALKING_FACTOR = 0.517;

        private double mCalories         = 0;
        private double mCaloriesAnterior = 0;

        #endregion

        private Manager manager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var t=Task.Factory.StartNew(() =>
            {
                manager = Manager.GetInstance(new SQLitePlatformAndroid(),
                new JsvServiceClient("http://prodactive.co/api"),
                System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                    "prodactive.db"));
            });
            lr = JsonSerializer.DeserializeFromString<LoginResponse>(Intent.GetStringExtra("UserData"));

            var estatura = lr.Persona.Estatura*100;
            GetStepLength(lr, estatura);
            BodyWeight = lr.Persona.Peso;
            

            string s = "UserData";
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Reto);

            //// Get our button from the layout resource,
            //// and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.btnIniciarReto);
            //Button Cerrar = FindViewById<Button>(Resource.Id.button1);
            tv            = FindViewById<TextView>(Resource.Id.txtPasos);
            tcalorias     = FindViewById<TextView>(Resource.Id.txtCalorias);
            

            //verificar si ya acepte el reto....
            button.Click += delegate
            {
                if (swButton)
                {
                    OnDestroy();
                    //button.Text = "Iniciar Reto";
                    swButton = false;
                }
                else
                {
                    if (lr.Reto.IsActivo)
                    {
                        
                        StartStepService();
                        Toast.MakeText(this, "El reto ha comenzado. La aplicación iniciará el conteo de pasos", ToastLength.Long);
                        Reporte.Start();
                        //button.Enabled = false;
                        button.Text = "Cerrar App";
                        swButton = true;
            
                    }
                }




                
            };
            //Cerrar.Click += (sender, e) =>
            //{
            //    Finish();

            //};
            handler = new Handler();
            t.Wait();

            Reporte = new System.Timers.Timer(60000);
            Reporte.Elapsed += EnviarReportePasos;


        }

        private void GetStepLength(LoginResponse lr, double estatura)
        {
            if (lr.Persona.Sexo == "M")
            {
                if (estatura != 0)
                    StepLength = estatura*0.415;
                else
                    StepLength = 78;
            }
            else
            {
                if (estatura == 0)
                    StepLength = estatura*0.413;
                else
                    StepLength = 70;
            }
        }

        protected override void OnStop   ()
        {
            base.OnStop();
            if (IsBound)
            {
                UnbindService(serviceConnection);
                IsBound = false;
            }
        }
        protected override void OnDestroy()
        {
            EnvioReporte();
            base.OnDestroy();
            if (IsBound)
            {
                UnbindService(serviceConnection);
                IsBound = false;
            }

            Finish();
        }
        protected override void OnStart  ()
        {
            base.OnStart();

            //if (!Utils.IsKitKatWithStepCounter(PackageManager))
            //{
            //    Log.Info("KitKat","Not compatible with sensors, stopping service.");
            //    return;
            //}

            if (!firstRun)
                StartStepService();

            if (IsBound)
                return;

            var serviceIntent = new Intent(this, typeof(StepService));
            serviceConnection = new StepServiceConnection(this);
            BindService(serviceIntent, serviceConnection, Bind.AutoCreate);
        }
        protected override void OnPause  ()
        {
            base.OnPause();
            if (registered && binder != null)
            {
                binder.StepService.PropertyChanged -= HandlePropertyChanged;
                registered = false;
            }
        }
        protected override void OnResume ()
        {
            base.OnResume();
            if (!firstRun)
            {

                if (handler == null)
                    handler = new Handler();
                handler.PostDelayed(() => UpdateUI(true), 500);
            }

            firstRun = false;

            if (!registered && binder != null)
            {
                binder.StepService.PropertyChanged += HandlePropertyChanged;
                registered = true;
            }
        }
        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "StepsToday")
                return;
            UpdateUI();
        }
        //update UI
        private void UpdateUI(bool force = false)
        {
            //if (progressView == null)
            //    return;
            lockMethod(()=>{

                Int64 steps = 0;
                var showWaring = false;
                if (Binder == null)
                {
                    if (Utils.IsSameDay)
                        steps = Helpers.Settings.CurrentDaySteps;
                }
                else
                {
                    steps = Binder.StepService.StepsToday;
                    showWaring = binder.StepService.WarningState;
                }

                mCalories += (BodyWeight * METRIC_WALKING_FACTOR //(mIsRunning ? METRIC_RUNNING_FACTOR : METRIC_WALKING_FACTOR))
                    // Distance:
                    * StepLength // centimeters
                    / 100000.0); // centimeters/kilometer
                contPasos = steps;

                //Task.Factory.StartNew(EnvioReporte); //solo para pruebas
                RunOnUiThread(() =>
                {
                    tv.Text = String.Format(" Pasos {0}  ", steps);
                    tcalorias.Text = String.Format("Calorias {0} ", contPasos); //Math.Floor(mCalories));
                    //progressView.SetStepCount(steps);

                    //stepCount.Text = Utils.FormatSteps(steps);

                    //var miles = Conversion.StepsToMiles(steps);
                    //distance.Text = string.Format(distanceString,
                    //    Helpers.Settings.UseKilometeres ?
                    //    Conversion.StepsToKilometers(steps).ToString("N2") :
                    //    miles.ToString("N2"));

                    //var lbs = Helpers.Settings.UseKilometeres ? Helpers.Settings.Weight * 2.20462 : Helpers.Settings.Weight;
                    //calorieCount.Text = string.Format(calorieString,
                    //    Helpers.Settings.Enhanced ?
                    //    Conversion.CaloriesBurnt(miles, (float)lbs, Helpers.Settings.Cadence) :
                    //    Conversion.CaloriesBurnt(miles));

                    //var percent = Conversion.StepCountToPercentage(steps);
                    //var percent2 = percent / 100;

                    //if (steps <= 10000)
                    //    percentage.Text = steps == 0 ? string.Empty : string.Format(percentString, percent2.ToString("P2"));
                    //else
                    //    percentage.Text = completedString;

                    ////set high score day
                    //highScore.Visibility = Settings.TodayIsHighScore ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Invisible;

                    ////detect warning
                    //warning.Visibility = showWaring ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Invisible;
                    ////Show daily goal message.
                    //if (!string.IsNullOrWhiteSpace(Settings.GoalTodayMessage) &&
                    //    Settings.GoalTodayDay.DayOfYear == DateTime.Today.DayOfYear &&
                    //    Settings.GoalTodayDay.Year == DateTime.Today.Year)
                    //{
                    //    Toast.MakeText(this, Settings.GoalTodayMessage, ToastLength.Long).Show();
                    //    Settings.GoalTodayMessage = string.Empty;
                    //}

                    //AnimateTopLayer((float)percent, force);

                    this.Title = Utils.DateString;
                });

            });
        }

        private void EnviarReportePasos(object sender, System.Timers.ElapsedEventArgs e)
        {
          EnvioReporte();
            
        }
        private void EnvioReporte()
        {
            lockMethod(() => {

                ReporteSql rep = new ReporteSql();
                rep.Calorias = mCalories;
                rep.Pasos = contPasos - pasosUltimaActualizacion;
                rep.UserName = lr.User;
                rep.Fecha = DateTime.Now;
                rep.IdReto = lr.Reto.Id;

                if (rep.Pasos > 0)
                {
                    bool sw = false;
                    //if (rep.Pasos > 0)
                    if (isOnline())
                    {
                        sw = manager.SendReporte(rep);
                        if (sw)
                        {
                            pasosUltimaActualizacion = contPasos;
                            mCaloriesAnterior = mCalories;
                            Toast.MakeText(this, "Se ha Enviado Reporte", ToastLength.Long);
                        }

                        Log.Info("Envio Reporte", String.Format("Se ha enviado reporte de pasos al servidor " + rep.Pasos));
                    }
                    else
                        sw = (manager.SaveReporte(rep) > 0 ? true : false);
                }
            });
        }

        private void StartStepService()
        {

            Task.Factory.StartNew(() =>
            {
                try
                {
                    service= new Intent(this, typeof(StepService));
                    var componentName = StartService(service);
                }
                catch (Exception ex)
                {
                }
            });
            
        }
        private void StopStepService()
        {

            Task.Factory.StartNew(() =>
            {
                try
                {
                    binder.StepService.StopSelf();
                    StopService(service);
                }
                catch (Exception ex)
                {
                }
            });

        }



        private void lockMethod(Action act)
        {
            lock (isSendingReporte)
            {
                if (!isSending)
                {

                    lock (isSendingReporte)
                    {
                        isSendingReporte = true;
                        try
                        {
                            act();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("LockMethod", ex.Message);
                        }
                        finally
                        {
                            isSendingReporte = false;
                        }
                    }
                }
            }
        }

        public
            bool isOnline()
        {
            ConnectivityManager cm = (ConnectivityManager) GetSystemService(Context.ConnectivityService);
            NetworkInfo netInfo = cm.ActiveNetworkInfo;
            if (netInfo != null && netInfo.IsConnected)
            {
                return true;
            }
            return false;
        }
    }
}