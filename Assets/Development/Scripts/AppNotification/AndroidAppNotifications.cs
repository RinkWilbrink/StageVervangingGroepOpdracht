using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

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

        AndroidNotificationCenter.CancelAllScheduledNotifications();

        #region IDK notification

        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Test Notification, help please",
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

        #endregion
    }

    private void OnApplicationPause(bool pause)
    {
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
        {
            AndroidNotification newNotification = new AndroidNotification()
            {
                Title = "pause Notification",
                Text = "help mij please!",
                SmallIcon = "app_icon_small",
                LargeIcon = "app_icon_large",
                FireTime = System.DateTime.Now.AddSeconds(5)
            };

            AndroidNotification b = NewNotification();

            AndroidNotificationCenter.UpdateScheduledNotification(identifier, newNotification, "default_channel");
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
        {
            AndroidNotificationCenter.CancelNotification(identifier);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Unknown)
        {
            AndroidNotification notification = new AndroidNotification()
            {
                Title = "pause Notification 2",
                Text = "help mij please!",
                SmallIcon = "app_icon_small",
                LargeIcon = "app_icon_large",
                FireTime = System.DateTime.Now.AddSeconds(5)
            };

            AndroidNotificationCenter.SendNotification(notification, "default_channel");
        }
    }

    private AndroidNotification NewNotification(string _title = "", string _text = "", string _smallIcon = "", string _largeIcon = "", int _fireTime = 0,
        string _group = "", GroupAlertBehaviours _groupBehaviour = GroupAlertBehaviours.GroupAlertAll, bool _groupSummary = true, 
        string _intentData = "", string _sortKey = "", NotificationStyle _style = NotificationStyle.None, bool _usesStopwatch = false)
    {
        return new AndroidNotification()
        {
            Group = _group,
            GroupAlertBehaviour = _groupBehaviour,
            GroupSummary = _groupSummary,
            IntentData = _intentData,
            LargeIcon = _largeIcon,
            SmallIcon = _smallIcon,
            SortKey = _sortKey,
            Style = _style,
            Text = _text,
            Title = _title,
            UsesStopwatch = _usesStopwatch
        };
    }
}
