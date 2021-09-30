using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private Data data = null;
    public Data CurrentData
    {
        get
        {
            return data;
        }
    }


    private string Save_Path = "";
    private string Save_FileName = "/SaveFile.txt";

    private void Awake()
    {
        Save_Path = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(Save_Path))
        {
            Directory.CreateDirectory(Save_Path);
        }
        LoadToJson();
        InvokeRepeating("SaveToJson", 1f, 60f);
        DontDestroyOnLoad(Instance);
    }

    private void LoadToJson()
    {
        if (File.Exists(Save_Path + Save_FileName))
        {
            string json = File.ReadAllText(Save_Path + Save_FileName);
            data = JsonUtility.FromJson<Data>(json);
        }
    }
    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Save_Path + Save_FileName, json, System.Text.Encoding.UTF8);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
