using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : NetworkTime
{
    [SerializeField] private float rewardTime = 86400000f;
    [SerializeField] private float updateCooldown = 1f; 
    [SerializeField] private Button rewardButton;
    [SerializeField] private Text timerText;
    private bool waitingForReward = false;
    private ulong lastTime;

    //[SerializeField] private AndroidAppNotifications appNotifications;

    private void Start() {
        print("Time: " + GetNetworkTime().ToLocalTime());

        lastTime = ulong.Parse(PlayerPrefs.GetString("LastTimeStamp"));

        if ( !IsRewardReady() ) {
            waitingForReward = true;
            rewardButton.interactable = false;
        }
    }

    float updateCooldownTimer = 60f;
    private void Update() {
        updateCooldownTimer += Time.deltaTime;

        if ( waitingForReward && updateCooldownTimer >= updateCooldown ) {
            if ( IsRewardReady() ) {
                waitingForReward = false;
                rewardButton.interactable = true;
                Debug.Log("Rewarded!");
                return;
            }

            UpdateTextTimer();
            updateCooldownTimer = 0f;
        }

        if ( Input.GetKeyDown(KeyCode.Y) ) {
            print("Time: " + GetNetworkTime().ToLocalTime());
        }
    }

    private bool IsRewardReady() {
        ulong diff = (ulong)GetNetworkTime().Ticks - lastTime;
        ulong ms = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = ( rewardTime - ms ) / 1000f;

        if ( secondsLeft < 0f ) {
            // Reward is ready to claim
            timerText.text = "";
            return true;
        }

        return false;
    }

    private void UpdateTextTimer() {
        ulong diff = (ulong)GetNetworkTime().Ticks - lastTime;
        ulong ms = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = ( rewardTime - ms ) / 1000f;
        string t = "";

        // Hours, Minutes, Seconds
        t += ( (int)secondsLeft / 3600 ) + "h ";
        secondsLeft -= ( (int)secondsLeft / 3600 ) * 3600;
        t += ( (int)secondsLeft / 60 ).ToString("00") + "m ";
        t += ( (int)secondsLeft % 60 ).ToString("00") + "s";

        timerText.text = t;

        // Schedule Notification
        //appNotifications.ScheduleNotification(System.DateTime.Now.AddSeconds(secondsLeft));
    }

    public void CheckTime() {
        Debug.Log("Setting up time...");

        lastTime = (ulong)GetNetworkTime().Ticks;
        PlayerPrefs.SetString("LastTimeStamp", lastTime.ToString());

        rewardButton.interactable = false;
        waitingForReward = true;

        GameController.Gems += 5;
        PlayerPrefs.SetInt("Gems", GameController.Gems);
    }

    //public static DateTime GetNetworkTime() {
    //    const string ntpServer = "time.windows.com";

    //    // NTP message size - 16 bytes of the digest (RFC 2030)
    //    var ntpData = new byte[48];

    //    // Setting the Leap Indicator, Version Number and Mode values
    //    ntpData[0] = 0x1B; // LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

    //    var addresses = Dns.GetHostEntry(ntpServer).AddressList;

    //    // The UDP port number assigned to NTP is 123
    //    var ipEndPoint = new IPEndPoint(addresses[0], 123);
    //    // NTP uses UDP

    //    using ( var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) ) {
    //        socket.Connect(ipEndPoint);

    //        // Stops code hang if NTP is blocked
    //        socket.ReceiveTimeout = 3000;

    //        socket.Send(ntpData);
    //        socket.Receive(ntpData);
    //        socket.Close();
    //    }

    //    // Offset to get to the "Transmit Timestamp" field (time at which the reply 
    //    // departed the server for the client, in 64-bit timestamp format.)
    //    const byte serverReplyTime = 40;

    //    // Get the seconds part
    //    ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

    //    // Get the seconds fraction
    //    ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

    //    // Convert From big-endian to little-endian
    //    intPart = SwapEndianness(intPart);
    //    fractPart = SwapEndianness(fractPart);

    //    var milliseconds = ( intPart * 1000 ) + ( ( fractPart * 1000 ) / 0x100000000L );

    //    // **UTC** time
    //    var networkDateTime = ( new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc) ).AddMilliseconds((long)milliseconds);

    //    return networkDateTime.ToLocalTime();
    //}

    //// stackoverflow.com/a/3294698/162671
    //private static uint SwapEndianness( ulong x ) {
    //    return (uint)( ( ( x & 0x000000ff ) << 24 ) +
    //                   ( ( x & 0x0000ff00 ) << 8 ) +
    //                   ( ( x & 0x00ff0000 ) >> 8 ) +
    //                   ( ( x & 0xff000000 ) >> 24 ) );
    //}
}
