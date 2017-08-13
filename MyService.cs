using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LearningServices
{
    [Service]
    public class MyService:Service
    {
        public static readonly string ServiceTimeKey = "ServiceTimeKey";
        bool _serviceRunning ;
        public override void OnCreate()
        {
            base.OnCreate();
            _serviceRunning = true;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            
            startBackgroundTask(intent, startId);
            return StartCommandResult.Sticky;
        }

        private void startBackgroundTask(Intent intent, int startId)
        {
            //TODO every 5 sec broadcast time
            Thread workingThread=new Thread(run);
            workingThread.Start();
        }

        protected void run()
        {
            Intent intent = new Intent("MyService");
            
            while (_serviceRunning)
            {
                intent.PutExtra(ServiceTimeKey, "the time is" + DateTime.Now.ToLongTimeString());
                SendBroadcast(intent);
                Thread.Sleep(2000);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _serviceRunning = false;

        }
    }
}