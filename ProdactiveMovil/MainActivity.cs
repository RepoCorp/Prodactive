using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Widget;
using Android.OS;
using ProdactiveMovil.Database;
using ProdactiveMovil.ModelServicio;
using ProdactiveMovil.ModelServicio.SQLite;
using ServiceStack;
using ServiceStack.Text;
using SQLite.Net.Platform.XamarinAndroid;

namespace ProdactiveMovil
{
    [Activity(Label = "ProdactiveMovil", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        private EditText txtUser;
        private EditText txtPass;

        private Manager manager;

        private JsvServiceClient client;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            client = new JsvServiceClient("http://prodactive.co/api");


            manager = Manager.GetInstance(
                new SQLitePlatformAndroid(),
                client,
                System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"prodactive.db"));
            // Get our button from the layout resource,
            // and attach an event to it
            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
             txtUser = FindViewById<EditText>(Resource.Id.txtUser);
             txtPass = FindViewById<EditText>(Resource.Id.txtPass);
            //Anadir Servicio

            btnLogin.Click += btnLogin_Click;

        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            
            LoginResponse lr = client.Send<LoginResponse>(new Login() { User = txtUser.Text, Pass = txtPass.Text });

            var t = Task.Factory.StartNew(() => manager.SavePersona(new PersonaSql(lr.Persona)));
            var r = Task.Factory.StartNew(() => manager.SaveReto(new RetoSql(lr.Reto)));
            if (lr.State)
            {
                //termino esta tarea
                Task.Factory.StartNew(()=>
                {
                    if (!lr.Reto.IsActivo)
                        lr.Reto.IsActivo = true;
                    Intent intent = new Intent(this, typeof(RetoActivity));
                    intent.PutExtra("UserData", JsonSerializer.SerializeToString(lr, typeof(LoginResponse)));
                    StartActivity(intent);
                });

                Task.WaitAll(new Task[] {t, r});
                Finish();
            }
        }


        public bool isOnline()
        {
            ConnectivityManager cm =
                (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo netInfo = cm.ActiveNetworkInfo;
            if (netInfo != null && netInfo.IsConnected)
            {
                return true;
            }
            return false;
        }
    }
}

