using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.IO;
using ProdactiveMovil.ModelServicio;
using ServiceStack;

namespace ProdactiveMovil
{
    [Activity(Label = "ProdactiveMovil", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        private EditText txtUser;
        private EditText txtPass;

        private JsvServiceClient client;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
             txtUser = FindViewById<EditText>(Resource.Id.txtUser);
             txtPass = FindViewById<EditText>(Resource.Id.txtPass);

             client = new JsvServiceClient("http://localhost:58640/api/");
            //Anadir Servicio

            btnLogin.Click += btnLogin_Click;

        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            ServiceL      sl = new ServiceL();
            LoginResponse lr = client.Send<LoginResponse>(
                                new Login() { User = txtUser.Text, Pass = txtPass.Text });
            int i = 0;
        }
    }

    public class ServiceL
    {
        public string Login(string User, string Pass)
        {
            return "";

        }
    }
}

