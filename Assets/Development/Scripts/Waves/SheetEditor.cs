using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

[CustomEditor(typeof(SheetData))]
public class SheetEditor : Editor
{
    private UnityWebRequest webRequest;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        GUILayout.Space(10);
        if ( GUILayout.Button("Update Yokai DataSheet", GUILayout.Height(25)) ) {
            goldRewardList.Clear();
            enemyCountList.Clear();

            webRequest = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbzskXjSJLDd69KdlIAFW_-LpFA60Xk587YbwSQP--s_NTi3ilvcqI24Bg/exec");
            webRequest.SendWebRequest();

            EditorApplication.update += CheckForImportRequestEnd;
        }
    }

    List<int> goldRewardList = new List<int>();
    List<int> enemyCountList = new List<int>();
    private void CheckForImportRequestEnd() {
        if ( webRequest != null && webRequest.isDone ) {
            WaveDataCollection data = JsonUtility.FromJson<WaveDataCollection>(webRequest.downloadHandler.text);
            SheetData sheetData = (SheetData)target;

            foreach ( WaveData item in data.jsonData ) {
                Debug.Log("GoldReward: " + item.goldReward);
                goldRewardList.Add(item.goldReward);
                Debug.Log("EnemyCount: " + item.enemyCount);
                enemyCountList.Add(item.enemyCount);
            }

            sheetData.GoldReward = goldRewardList.ToArray();
            sheetData.EnemyCount = enemyCountList.ToArray();

            Debug.Log(webRequest.downloadHandler.text);

            EditorApplication.update -= CheckForImportRequestEnd;
            Repaint();
        }
    }
}
