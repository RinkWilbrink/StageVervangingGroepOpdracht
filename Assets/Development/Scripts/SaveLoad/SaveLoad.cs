using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using CryptSharp.Utility;

public class SaveLoad : MonoBehaviour
{
    //GameManager _GameManager;
    System.Threading.Thread _SaveThread = null;
    System.Threading.Thread _LoadThread = null;

    string _DataPath;

    private void Start()
    {
        //_GameManager = GetComponent<GameManager>();
        _SaveThread = new System.Threading.Thread(SaveThread);

        _DataPath = Application.persistentDataPath + "/save.txt";

        _LoadThread = new System.Threading.Thread(LoadData);
        _LoadThread.Start();
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
                EnterEmptyValues();
                return;
            }
            
            //_GameManager.SetZap(saveObject._Zap);
            //_GameManager.SetHighScore(saveObject._HighScore);
            //_GameManager.SetColor(saveObject._ActiveColor);

            CheckIfValuesEmpty(saveObject);

            print("loaded");
        }
        else
        {
            EnterEmptyValues();
            print("no save");
        }

        _LoadThread = new System.Threading.Thread(LoadData);
    }

    private string GetHashCode(SaveObject saveObject)
    {
        bool[] colorsBought = saveObject._ColorBought;
        string hashCode = saveObject._HighScore.ToString() + saveObject._Zap.ToString() + saveObject._ActiveColor.ToString();

        for (int i = 0; i < colorsBought.Length; i++)
        {
            hashCode += colorsBought.ToString();
        }

        return Hash(hashCode, "NagitoLovesBagles");
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
            //_HighScore = _GameManager.GetHighScore(),
            //_Zap = _GameManager.GetZapAmount(),
            //_ColorBought = _GameManager.GetShopColorValues(),
            //_ActiveColor = _GameManager.GetColor(),
        };

        saveObject._Hash = GetHashCode(saveObject);

        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(_DataPath, json);

        print("saved");

        _SaveThread = new System.Threading.Thread(SaveThread);
    }


    private void EnterEmptyValues()
    {
        //_GameManager.SetShopColorValues(new bool[3]);
        //_GameManager.SetShopColorValue(0, true);
        //_GameManager.SetColor(new Color(0.5058824f, 1, 0.4862745f, 1)); //Pastel Green
    }

    private void CheckIfValuesEmpty(SaveObject saveObject)
    {
        //if (_GameManager.GetShopColorValues() != saveObject._ColorBought)
        //{
        //    _GameManager.SetShopColorValues(new bool[3]);
        //}

        //if (_GameManager.GetColor() != saveObject._ActiveColor)
        //{
        //    _GameManager.SetColor(new Color(0.5058824f, 1, 0.4862745f, 1)); //Pastel Green
        //}
    }

    public bool GetLoadThread()
    {
        return _LoadThread.IsAlive;
    }
}

[Serializable]
public struct SaveObject
{
    public int _HighScore;
    public int _Zap;

    public bool[] _ColorBought;
    public Color _ActiveColor;

    public string _Hash;
}
