using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace LearningServices
{
    [Activity(Label = "LearningServices", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button _button;
        bool _serviceStarted;
        TimeReceiver _timeReceiver;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            registerRecevers();

            _button = FindViewById<Button>(Resource.Id.MyButton);
            _button.Click += _button_Click;
        }

        private void registerRecevers()
        {
            _timeReceiver=new TimeReceiver(this);
            RegisterReceiver(_timeReceiver, new IntentFilter("MyService"));
        }

        void _button_Click(object sender, EventArgs e)
        {
            if (_serviceStarted)
            {
                stopService();
            }
            else
            {
                startService();
            }
        }

        private void stopService()
        {
            StopService(new Intent(this, typeof(MyService)));
            _serviceStarted = false;
            UpdateMessage("Service Stopped");
        }

        private void startService()
        {
            StartService(new Intent(this, typeof(MyService)));
            _serviceStarted = true;
            UpdateMessage("Service Started");
        }

        public void UpdateMessage(string message)
        {
            _button.Text ="service="+_serviceStarted+" and message is:"+ message;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            stopService();
            unregisterReceivers();
            
        }

        private void unregisterReceivers()
        {
            UnregisterReceiver(_timeReceiver);
        }
    }

    public class TimeReceiver : BroadcastReceiver
    {
        MainActivity _mainActivity;
        public TimeReceiver(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }
        public override void OnReceive(Context context, Intent intent)
        {

            string message = "noData";
            if (intent.Extras != null)
                message = intent.Extras.GetString(MyService.ServiceTimeKey, "noData");
            _mainActivity.UpdateMessage(message);
        }
    }
}

