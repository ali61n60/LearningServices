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
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            Notification.Builder nBuilder = new Notification.Builder(ApplicationContext);
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);
            // set intent so it does not start a new activity
            Notification notification = nBuilder.SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetWhen(DateTime.Now.Ticks)
                .SetProgress(100,25,false)
                .SetContentTitle("Demo Service Notification")
                .SetContentText("Message from demo service").Build();
            notification.Flags |= NotificationFlags.AutoCancel;
            notificationManager.Notify(0, notification);

        }
       

        public override void OnDestroy()
        {
            base.OnDestroy();
            _serviceRunning = false;

        }
    }
}