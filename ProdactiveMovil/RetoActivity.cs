using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.W3c.Dom;
using ProdactiveMovil.Helpers;
using ProdactiveMovil.ModelServicio;
using ProdactiveMovil.Services;
using ServiceStack.Text;

namespace ProdactiveMovil
{
    [Activity(Label = "RetoActivity")]
    public class RetoActivity : Activity
    {
        int count = 1;
        private StepServiceBinder binder;
        
        private StepServiceConnection serviceConnection;
        private bool firstRun = true;
        private bool registered;
        private Handler handler;
        public bool IsBound { get; set; }
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

        private double StepLength = 0;



        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            
            LoginResponse lr = JsonSerializer.DeserializeFromString<LoginResponse>(Intent.GetStringExtra("UserData"));

            var estatura = lr.Persona.Estatura*100;
            if (lr.Persona.Sexo == "M")
            {
                if(estatura!=0)
                    StepLength = estatura*0.415;
                else
                    StepLength = 78;
            }
            else
            {
                if(estatura==0)
                    StepLength = estatura * 0.413;
                else
                    StepLength = 70;
            }
            
            

            string s = "UserData";
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Reto);

            //// Get our button from the layout resource,
            //// and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.btnIniciarReto);

            //Button Cerrar = FindViewById<Button>(Resource.Id.button1);
            tv = FindViewById<TextView>(Resource.Id.txtPasos);

            button.Click += delegate
            {
                StartStepService();
                Toast.MakeText(this, "El reto ha comenzado. La aplicación iniciará el conteo de pasos", ToastLength.Long);
            };
            //Cerrar.Click += (sender, e) =>
            //{
            //    Finish();

            //};
            handler = new Handler();
            

        }
        protected override void OnStop()
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
            base.OnDestroy();
            if (IsBound)
            {
                UnbindService(serviceConnection);
                IsBound = false;
            }
        }
        protected override void OnStart()
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
        protected override void OnPause()
        {
            base.OnPause();
            if (registered && binder != null)
            {
                binder.StepService.PropertyChanged -= HandlePropertyChanged;
                registered = false;
            }
        }
        protected override void OnResume()
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
            RunOnUiThread(() =>
            {
                tv.Text = String.Format("Numero Pasos Dia de Hoy {0}", steps);
                
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
        }
        private void StartStepService()
        {

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var service = new Intent(this, typeof(StepService));
                    var componentName = StartService(service);
                }
                catch (Exception ex)
                {
                }
            });
            
        }
        
    }
}