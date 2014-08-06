using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.IO;
using ProdactiveMovil.ModelServicio;
using ServiceStack;
using ServiceStack.Text;

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

             client = new JsvServiceClient("http://prodactive.co/api/");
            //Anadir Servicio

            btnLogin.Click += btnLogin_Click;

        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            //#if DEBUG
            //    LoginResponse lr = new LoginResponse() {User = "ddo88", State = true, Message = ""};
            //#else
                LoginResponse lr = client.Send<LoginResponse>(new Login() { User = txtUser.Text, Pass = txtPass.Text });
            //#endif


            if (lr.State)
            {
                //termino esta tarea
                Finish();
                Task.Factory.StartNew(()=>
                {
                    Intent intent = new Intent(this, typeof(RetoActivity));
                    intent.PutExtra("UserData", JsonSerializer.SerializeToString(lr, typeof(LoginResponse)));
                    StartActivity(intent);
                });
            }
            int i = 0;
        }
    }

}

