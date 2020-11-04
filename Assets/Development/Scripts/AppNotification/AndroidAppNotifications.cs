using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;
//using UnityEngine.Assertions;

public class AndroidAppNotifications : MonoBehaviour
{
    // Variables
    private AndroidNotificationChannel defaultNotificationChannel;
    private int identifier;

    void Start()
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Description = "For Generic Notifications",
            Importance = Importance.Default
        };

        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Test Notification",
            Text = "Banaan!",
            SmallIcon = "app_icon_small",
            LargeIcon = "app_icon_large",
            FireTime = System.DateTime.Now.AddSeconds(5)
        };

        identifier = AndroidNotificationCenter.SendNotification(notification, "channel_default");

        AndroidNotificationCenter.NotificationReceivedCallback receiveCallbackHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = string.Format("Notification Reveived : {0}\n", data.Id);
            msg += string.Format("\n Notification Reveived: \n .Title: {0}\n .Body: {1} \n .Channel: {2}", data.Notification.Title, data.Notification.Text, data.Channel);
        };

        AndroidNotificationCenter.OnNotificationReceived += receiveCallbackHandler;

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if(notificationIntentData != null)
        {
            Debug.Log("App was opened with Notification");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
        {
            AndroidNotification newNotification = new AndroidNotification()
            {
                Title = "Test Notification",
                Text = "Banaan!",
                SmallIcon = "app_icon_small",
                LargeIcon = "app_icon_large",
                FireTime = System.DateTime.Now.AddSeconds(5)
            };

            AndroidNotificationCenter.UpdateScheduledNotification(identifier, newNotification, "default_channel");
        }
        else if(AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
        {
            AndroidNotificationCenter.CancelNotification(identifier);
        }
        else if(AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Unknown)
        {
            AndroidNotification notification = new AndroidNotification()
            {
                Title = "Test Notification",
                Text = "Banaan!",
                SmallIcon = "app_icon_small",
                LargeIcon = "app_icon_large",
                FireTime = System.DateTime.Now.AddSeconds(5)
            };

            AndroidNotificationCenter.SendNotification(notification, "default_channel");
        }
    }
}
