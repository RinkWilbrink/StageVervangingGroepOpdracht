using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyChallenges : NetworkTime
{
    [SerializeField] private DailyChallenge[] challenges;
    [SerializeField] private int challenageAmount;
    public DailyChallenge challenge;
    [SerializeField] private float rotationTime = 86400000f;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    private bool waitingForNextChallenge = false;
    private ulong lastTime;

    private void Start() {
        //// Could try to call this only when starting the game instead
        //DontDestroyOnLoad(this.gameObject);

        print("Time: " + GetNetworkTime().ToLocalTime());
        //CheckTime();

        lastTime = ulong.Parse(PlayerPrefs.GetString("ChallengeTimeStamp"));

        for ( int i = 0; i < challenges.Length; i++ ) {
            //for ( int y = 0; y < challenageAmount; y++ ) {
            if ( challenges[i].challengeName == PlayerPrefs.GetString("TestChallenge") ) {
                challenge = challenges[i];
            }
            //}
        }
        print(challenge.challengeName);

        if ( !IsChallengeReady() ) {
            waitingForNextChallenge = true;
        }
    }

    private void Update() {
        //Debug.Log("Gems: " + GameController.Gems);

        if ( waitingForNextChallenge ) {
            if ( IsChallengeReady() ) {
                waitingForNextChallenge = false;
                Debug.Log("Swapping challenge(s)!");
                return;
            }

            UpdateTextTimer();
        }

        if ( Input.GetKeyDown(KeyCode.Y) ) {
            print("Time: " + GetNetworkTime().ToLocalTime());
        }
    }

    public void UpdateChallenge( DailyChallenge dailyChallenge ) {
        dailyChallenge.progress++;
    }

    private void OnDestroy() {
        CheckTime();
        // Save progress here

    }

    private bool IsChallengeReady() {
        ulong diff = (ulong)GetNetworkTime().Ticks - lastTime;
        ulong ms = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = ( rotationTime - ms ) / 1000f;

        if ( secondsLeft < 0f ) {
            // Swap challenge
            challenge = challenges[UnityEngine.Random.Range(0, challenges.Length)];
            print(challenge.challengeName);
            PlayerPrefs.SetString("TestChallenge", challenge.challengeName);
            PlayerPrefs.SetInt("TestChallengeProg", challenge.progress);
            CheckTime();
            return true;
        }

        return false;
    }

    private void UpdateTextTimer() {
        ulong diff = (ulong)GetNetworkTime().Ticks - lastTime;
        ulong ms = diff / TimeSpan.TicksPerMillisecond;
        float secondsLeft = ( rotationTime - ms ) / 1000f;
        string t = "";

        // Hours, Minutes, Seconds
        t += ( (int)secondsLeft / 3600 ) + "h ";
        secondsLeft -= ( (int)secondsLeft / 3600 ) * 3600;
        t += ( (int)secondsLeft / 60 ).ToString("00") + "m ";
        t += ( (int)secondsLeft % 60 ).ToString("00") + "s";

        timerText.text = t;
    }

    private void CheckTime() {
        Debug.Log("Setting up time for challenge...");

        lastTime = (ulong)GetNetworkTime().Ticks;
        PlayerPrefs.SetString("ChallengeTimeStamp", lastTime.ToString());

        challenge.progress = 0;

        waitingForNextChallenge = true;
    }
}
