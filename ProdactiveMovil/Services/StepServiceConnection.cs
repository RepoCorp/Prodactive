using Android.Content;
using Android.OS;

namespace ProdactiveMovil.Services
{
    public class StepServiceConnection : Java.Lang.Object, IServiceConnection
    {
        RetoActivity activity;

        public StepServiceConnection(RetoActivity activity)
        {
            this.activity = activity;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var serviceBinder = service as StepServiceBinder;
            if (serviceBinder != null)
            {
                activity.Binder = serviceBinder;
                activity.IsBound = true;
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            activity.IsBound = false;
        }
    }
}