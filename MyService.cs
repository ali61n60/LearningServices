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
           
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (!_serviceRunning)
            {
                _serviceRunning = true;
                startBackgroundTask(intent, startId);
            }

            return StartCommandResult.Sticky;
        }

        private async void startBackgroundTask(Intent intent, int startId)
        {
            //TODO every 5 sec broadcast time
            Intent intentService = new Intent("MyService");

            while (_serviceRunning)
            {
                intentService.PutExtra(ServiceTimeKey, "the time is" + DateTime.Now.ToLongTimeString());
                SendBroadcast(intentService);
                sendNotification();
                await Task.Delay(5000);
            }
        }

        private void sendNotification()
        {
            var nMgr = (NotificationManager)GetSystemService(NotificationService);
            var notification = new Notification(Resource.Drawable.Icon, "Message from MyService");
            var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);
            notification.SetLatestEventInfo(this, "Demo Service Notification", "Message from demo service", pendingIntent);
            nMgr.Notify(0, notification);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _serviceRunning = false;

        }
    }
}