using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using CryptSharp.Utility;

public class SaveLoad : MonoBehaviour
{
    System.Threading.Thread _SaveThread = null;
    System.Threading.Thread _LoadThread = null;

    string _DataPath;

    private void Start()
    {
        _SaveThread = new System.Threading.Thread(SaveThread);
        print(Application.persistentDataPath);

        _DataPath = Application.persistentDataPath + "/save.txt";

        _LoadThread = new System.Threading.Thread(LoadData);
        _LoadThread.Start();

        DataManager._SaveLoad = this; 
    }

    public void SaveData()
    {
        if (_SaveThread.IsAlive == false)
        {
            _SaveThread.Start();
            print("Saving Started");
        }
        else
        {
            print("Saving In Progress");
        }
    }

    public void LoadData()
    {
        if (File.Exists(_DataPath))
        {
            string saveString = File.ReadAllText(_DataPath);

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            if(saveObject._Hash != GetHashCode(saveObject))
            {
                print("Hash not the same");
                return;
            }

            DataManager._LastLevelBeaten = saveObject._LastLevelBeaten;
            DataManager._EnemiesKilled = saveObject._EnemiesKilled;
            DataManager._ResourcesGathered = saveObject._ResourcesGathered;
            DataManager._TowersPlaced = saveObject._TowersPlaced;


            print("loaded");
        }
        else
        {
            print("no save");
        }

        _LoadThread = new System.Threading.Thread(LoadData);
    }

    private string GetHashCode(SaveObject saveObject)
    {
        string hashCode = saveObject._LastLevelBeaten.ToString() + saveObject._EnemiesKilled.ToString() + saveObject._ResourcesGathered.ToString() + saveObject._ResourcesGathered.ToString();

        return Hash(hashCode, "JapaneseBrushStrokes");
    }

    private string Hash(string secret, string salt)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var cost = 262144;
        var blockSize = 8;
        var parallel = 1;
        var maxThreads = (int?)null;
        var derivedKeyLength = 128;

        var bytes = SCrypt.ComputeDerivedKey(keyBytes, saltBytes, cost, blockSize, parallel, maxThreads, derivedKeyLength);
        return Convert.ToBase64String(bytes);
    }

    private void SaveThread()
    {
        SaveObject saveObject = new SaveObject
        {
            _LastLevelBeaten = DataManager._LastLevelBeaten,
            _EnemiesKilled = DataManager._EnemiesKilled,
            _ResourcesGathered = DataManager._ResourcesGathered,
            _TowersPlaced = DataManager._TowersPlaced
        };

        saveObject._Hash = GetHashCode(saveObject);

        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(_DataPath, json);

        print("saved");

        _SaveThread = new System.Threading.Thread(SaveThread);
    }

    public bool GetLoadThread()
    {
        return _LoadThread.IsAlive;
    }
}

[Serializable]
public struct SaveObject
{
    public int _LastLevelBeaten;

    public int _EnemiesKilled;
    public int _ResourcesGathered;
    public int _TowersPlaced;

    public string _Hash;
}
