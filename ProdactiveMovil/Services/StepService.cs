using System;
using System.ComponentModel;
using System.Security.Cryptography;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Support.V4.App;
using Android.Util;
using Java.Util;
using ProdactiveMovil.Database;
using ProdactiveMovil.Helpers;

namespace ProdactiveMovil.Services
{
    [Service(Enabled = true)]
    [IntentFilter(new String[] {"com.refractored.mystepcounter.StepService"})]
    public class StepService : Service, ISensorEventListener, INotifyPropertyChanged
    {

        private static readonly string TAG = "com.refractored.mystepcounter.StepService";


        private System.Timers.Timer t;
        #region varAcelerometer

        private float Limit = 10;
        private float[] LastValues = new float[3*2];
        private float[] Scale = new float[2];
        private float YOffset;

        private float[] LastDirections = new float[3*2];
        private float[][] LastExtremes = {new float[3*2], new float[3*2]};
        private float[] LastDiff = new float[3*2];
        private int LastMatch = -1;

        #endregion

        private bool isRunning;
        private Int64 stepsToday = 0;
        private StepServiceBinder binder;
        private Int64 newSteps = 0;
        private Int64 lastSteps = 0;

        public bool WarningState { get; set; }

        public Int64 StepsToday
        {
            get { return stepsToday; }
            set
            {
                if (stepsToday == value)
                    return;

                stepsToday = value;
                OnPropertyChanged("StepsToday");
                Helpers.Settings.CurrentDaySteps = value;
            }
        }


        public StepService()
        {
            int h = 480; // TODO: remove this constant
            YOffset = h*0.5f;
            Scale[0] = -(h*0.5f*(1.0f/(SensorManager.StandardGravity*2)));
            Scale[1] = -(h*0.5f*(1.0f/(SensorManager.MagneticFieldEarthMax)));

            //esto solo para simular pasos
            t = new System.Timers.Timer(1000);
            t.Elapsed += t_Elapsed;
                
        }

        private void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                System.Random r = new System.Random(DateTime.Now.Millisecond);

                AddSteps(lastSteps + 1);
            }
            catch (Exception ex)
            {

            }

        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //solo para debug
            t.Start();


            Console.WriteLine("StartCommand Called, setting alarm");
            #if DEBUG
            Log.Debug("STEPSERVICE", "Start command result called, incoming startup");
            #endif

            var alarmManager = ((AlarmManager)ApplicationContext.GetSystemService(AlarmService));
            var intent2 = new Intent(this, typeof(StepService));
            intent2.PutExtra("warning", WarningState);
            var stepIntent = PendingIntent.GetService(ApplicationContext, 10, intent2, PendingIntentFlags.UpdateCurrent);
            // Workaround as on Android 4.4.2 START_STICKY has currently no
            // effect
            // -> restart service every 60 mins
            alarmManager.Set(AlarmType.Rtc, Java.Lang.JavaSystem.CurrentTimeMillis() + 1000 * 60 * 60, stepIntent);
            
            var warning = false;
            if (intent != null)
                warning = intent.GetBooleanExtra("warning", false);
            Startup();

            return StartCommandResult.Sticky;
        }

        //public override void OnTaskRemoved(Intent rootIntent)
        //{
        //    base.OnTaskRemoved(rootIntent);

        //    UnregisterListeners();
        //    #if DEBUG
        //    Console.WriteLine("OnTaskRemoved Called, setting alarm for 500 ms");
        //    Android.Util.Log.Debug("STEPSERVICE", "Task Removed, going down");
        //    #endif
        //    var intent = new Intent(this, typeof(StepService));
        //    intent.PutExtra("warning", WarningState);
        //    // Restart service in 500 ms
        //    ((AlarmManager)GetSystemService(Context.AlarmService)).Set(AlarmType.Rtc, Java.Lang.JavaSystem
        //        .CurrentTimeMillis() + 500,
        //        PendingIntent.GetService(this, 11, intent, 0));
        //}

        private void Startup(bool warning = false)
        {
            //check if kit kat can sensor compatible
            //if (!Utils.IsKitKatWithStepCounter(PackageManager))
            //{
            //    Console.WriteLine("Not compatible with sensors, Loading Accelerometer.");
            //    //Console.WriteLine ("Not compatible with sensors, stopping service.");
            //    //				StopSelf ();
            //    //				return;

            //    CrunchDates(true);
            //    if (!isRunning)
            //    {
            //        RegisterListeners(warning ? SensorType.StepCounter : SensorType.Accelerometer);
            //        WarningState = warning;
            //    }

            //    isRunning = true;
            //}
            //else
            //{
            CrunchDates(true);

            if (!isRunning)
            {
                //RegisterListeners(warning ? SensorType.Accelerometer:SensorType.StepDetector); //StepDetector : SensorType.StepCounter);
                RegisterListeners(SensorType.Accelerometer); //StepDetector : SensorType.StepCounter);
                WarningState = warning;
            }

            isRunning = true;
            //}

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterListeners();
            isRunning = false;
            CrunchDates();
        }
        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            binder = new StepServiceBinder(this);
            return binder;
        }

        void RegisterListeners(SensorType sensorType)
        {

            var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
            var sensor = sensorManager.GetDefaultSensor(sensorType);
            
            //get faster why not, nearly fast already and when
            //sensor gets messed up it will be better
            sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
            Console.WriteLine("Sensor listener registered of type: " + sensorType);

        }
        void UnregisterListeners()
        {

            if (!isRunning)
                return;

            try
            {
                var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
                sensorManager.UnregisterListener(this);
                Console.WriteLine("Sensor listener unregistered.");
                #if DEBUG
                    Android.Util.Log.Debug("STEPSERVICE", "Sensor listener unregistered.");
                #endif
                isRunning = false;
            }
            catch (Exception ex)
            {
                #if DEBUG
                   Android.Util.Log.Debug("STEPSERVICE", "Unable to unregister: " + ex);
                #endif
            }
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //do nothing here
        }
        public void AddSteps(Int64 count)
        {
            
            //if service rebooted or rebound then this will null out to 0, but count will still be since last boot.
            if (lastSteps == 0)
            {
                lastSteps = count;
            }

            //calculate new steps
            newSteps = count - lastSteps;


            Log.Info("Numero Pasos", String.Format("Pasos {0} - Pasos Anterior {1}", newSteps,lastSteps));
            //ensure we are never negative
            //if so, no worries as we are about to re-set the lastSteps to the
            //current count
            if (newSteps < 0)
                newSteps = 1;

            lastSteps = count;

            //ensure we don't need to re-boot day :)
            CrunchDates();
            CrunchHighScores();

            //save total steps!
            Helpers.Settings.TotalSteps += newSteps;

            StepsToday = Helpers.Settings.TotalSteps - Helpers.Settings.StepsBeforeToday;

            Console.WriteLine("New step detected by STEP_COUNTER sensor. Total step count: " + stepsToday);
            #if DEBUG
                Android.Util.Log.Debug("STEPSERVICE", "New steps: " + newSteps + " total: " + stepsToday);
            #endif
        }
        public void OnSensorChanged(SensorEvent e)
        {
            switch (e.Sensor.Type)
            {
                case SensorType.Accelerometer:
                {
                    float vSum = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        float vv = YOffset + e.Values[i] * Scale[1];
                        vSum += vv;
                    }
                    int k = 0;
                    float v = vSum / 3;

                    float direction = (v > LastValues[k] ? 1 : (v < LastValues[k] ? -1 : 0));
                    if (direction == -LastDirections[k])
                    {
                        // Direction changed
                        int extType = (direction > 0 ? 0 : 1); // minumum or maximum?
                        LastExtremes[extType][k] = LastValues[k];

                        float diff = Math.Abs(LastExtremes[extType][k] - LastExtremes[1 - extType][k]);

                        if (diff > Limit)
                        {

                            bool isAlmostAsLargeAsPrevious = diff > (LastDiff[k] * 2 / 3);
                            bool isPreviousLargeEnough = LastDiff[k] > (diff / 3);
                            bool isNotContra = (LastMatch != 1 - extType);

                            if (isAlmostAsLargeAsPrevious && isPreviousLargeEnough && isNotContra)
                            {
                                Log.Info(TAG, "step");


                                AddSteps(lastSteps + 1);
                                //foreach (IStepListener stepListener in ListStepListeners)
                                //{
                                //    stepListener.OnStep();
                                //}
                                LastMatch = extType;
                            }
                            else
                            {
                                LastMatch = -1;
                            }
                        }
                        LastDiff[k] = diff;
                    }
                    LastDirections[k] = direction;
                    LastValues[k] = v;
                }
                    break;
                    //case SensorType.StepCounter:

                    //    if (lastSteps < 0)
                    //        lastSteps = 0;

                    //    //grab out the current value.
                    //    var count = (Int64)e.Values[0];
                    //    //in some instances if things are running too long (about 4 days)
                    //    //the value flips and gets crazy and this will be -1
                    //    //so switch to step detector instead, but put up warning sign.
                    //    if (count < 0)
                    //    {

                    //        UnregisterListeners();
                    //        RegisterListeners(SensorType.StepDetector);
                    //        isRunning = true;
                    //        #if DEBUG
                    //        Log.Debug("STEPSERVICE", "Something has gone wrong with the step counter, simulating steps, 2.");
                    //        #endif
                    //        count = lastSteps + 3;

                    //        WarningState = true;
                    //    }
                    //    else
                    //    {
                    //        WarningState = false;
                    //    }

                    //    AddSteps(count);

                    //    break;
                    //case SensorType.StepDetector:
                    //    count = lastSteps + 1;
                    //    AddSteps(count);
                    //    break;
            }
        }

        private void CrunchHighScores()
        {
            bool notification = Helpers.Settings.ProgressNotifications;

            int halfGoal = 5000;
            int fullGoal = 10000;
            int doubleGoal = 20000;
            if (stepsToday < halfGoal && stepsToday + newSteps >= halfGoal)
            {
                Helpers.Settings.GoalTodayDay = DateTime.Today;
                Helpers.Settings.GoalTodayMessage = Resources.GetString(Resource.String.goal_half);
            }
            else if (stepsToday < fullGoal && stepsToday + newSteps >= fullGoal)
            {
                Helpers.Settings.GoalTodayDay = DateTime.Today;
                Helpers.Settings.GoalTodayMessage = string.Format(Resources.GetString(Resource.String.goal_full), (fullGoal).ToString("N0"));
            }
            else if (stepsToday < doubleGoal && stepsToday + newSteps >= doubleGoal)
            {
                Helpers.Settings.GoalTodayDay = DateTime.Today;
                Helpers.Settings.GoalTodayMessage = string.Format(Resources.GetString(Resource.String.goal_double), (doubleGoal).ToString("N0"));
            }
            else
            {
                notification = false;
            }

            if (notification)
            {
                PopUpNotification(0, Resources.GetString(Resource.String.goal_update), Helpers.Settings.GoalTodayMessage);
            }

            notification = false;
            if (stepsToday + newSteps > Helpers.Settings.HighScore)
            {
                Helpers.Settings.HighScore = stepsToday + newSteps;
                //if not today
                if (!Helpers.Settings.TodayIsHighScore)
                {
                    //if first day of use then no notifications, else pop it up
                    if (Helpers.Settings.FirstDayOfUse.DayOfYear == DateTime.Today.DayOfYear &&
                        Helpers.Settings.FirstDayOfUse.Year == DateTime.Today.Year)
                    {
                        notification = false;
                    }
                    else
                    {
                        notification = Helpers.Settings.ProgressNotifications;
                    }
                }
                //this triggers a new high score day so the next tiem it comes in TodayIsHighScore will be true
                Helpers.Settings.HighScoreDay = DateTime.Today;
            }

            //notifcation for high score
            if (notification)
            {
                PopUpNotification(1, Resources.GetString(Resource.String.high_score_title),
                    string.Format(Resources.GetString(Resource.String.high_score),
                        Utils.FormatSteps(Helpers.Settings.HighScore)));
            }

            notification = Helpers.Settings.AccumulativeNotifications;
            var notificationString = string.Empty;
            if (Helpers.Settings.TotalSteps + newSteps > Helpers.Settings.NextGoal)
            {
                notificationString = string.Format(Resources.GetString(Resource.String.awesome), Utils.FormatSteps(Helpers.Settings.NextGoal));
                if (Helpers.Settings.NextGoal < 500000)
                {
                    Helpers.Settings.NextGoal = 500000;
                }
                else if (Helpers.Settings.NextGoal < 1000000)
                {
                    Helpers.Settings.NextGoal = 1000000;
                }
                else
                {
                    Helpers.Settings.NextGoal += 1000000;
                }
            }
            else
            {
                notification = false;
            }

            //notifcation for accumulative records
            if (notification)
            {
                PopUpNotification(2, Resources.GetString(Resource.String.awesome_title), notificationString);
            }

        }
        private void PopUpNotification(int id, string title, string message)
        {
            Notification.Builder mBuilder =
                new Notification.Builder(this)
                    //.SetSmallIcon(Resource.Drawable.ic_notification)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetAutoCancel(true);
            // Creates an explicit intent for an Activity in your app
            Intent resultIntent = new Intent(this, typeof(MainActivity));
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            // The stack builder object will contain an artificial back stack for the
            // started Activity.
            // This ensures that navigating backward from the Activity leads out of
            // your application to the Home screen.
            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
            // Adds the back stack for the Intent (but not the Intent itself)
            //stackBuilder.AddParentStack();
            // Adds the Intent that starts the Activity to the top of the stack
            stackBuilder.AddNextIntent(resultIntent);
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
            mBuilder.SetContentIntent(resultPendingIntent);



            NotificationManager mNotificationManager =
                (NotificationManager)GetSystemService(Context.NotificationService);
            // mId allows you to update the notification later on.
            //mNotificationManager.Notify(id, mBuilder.Build());

            mNotificationManager.Notify(id, mBuilder.Notification);
        }
        private void CrunchDates(bool startup = false)
        {
            if (!Utils.IsSameDay)
            {
                //save our day from yesterday, we dont' do datetime.adddays(-1) because phone might have been off
                //for more then 1 day and it would not be correct!
                var yesterday = Helpers.Settings.CurrentDay;
                var dayEntry = StepEntryManager.GetStepEntry(yesterday);
                if (dayEntry == null || dayEntry.Date.DayOfYear != yesterday.DayOfYear)
                {
                    dayEntry = new StepEntry();
                }

                dayEntry.Date = yesterday;
                dayEntry.Steps = Helpers.Settings.CurrentDaySteps;

                Helpers.Settings.CurrentDay = DateTime.Today;
                Helpers.Settings.CurrentDaySteps = 0;
                Helpers.Settings.StepsBeforeToday = Helpers.Settings.TotalSteps;
                StepsToday = 0;
                try
                {
                    StepEntryManager.SaveStepEntry(dayEntry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Something horrible has gone wrong attempting to save database entry, it is lost forever :(");
                }

            }
            else if (startup)
            {
                StepsToday = Helpers.Settings.TotalSteps - Helpers.Settings.StepsBeforeToday;
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}