using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private async Task startBackgroundTask(Intent intent, int startId)
        {
            //TODO every 5 sec broadcast time
            Intent intentService = new Intent("MyService");

            while (_serviceRunning)
            {
                intentService.PutExtra(ServiceTimeKey, "the time is" + DateTime.Now.ToLongTimeString());
                SendBroadcast(intentService);
               await Task.Delay(2000);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _serviceRunning = false;

        }
    }
}