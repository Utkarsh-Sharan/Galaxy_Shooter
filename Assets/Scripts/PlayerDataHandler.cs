using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{
    //singleton
    private static PlayerDataHandler _instance;
    public static PlayerDataHandler Instance
    {
        get
        {
            if(_instance == null)
            {            
                return _instance;
            }
            return null;
        }
        set
        {
            _instance = value;
        }
    }

    //current player data
    public string currentPlayerName;
    public int currentPlayerScore;
    public int currentPlayerEnemiesKilled;

    //best player data
    public string bestPlayerName;
    public int bestPlayerScore;
    public int bestPlayerEnemiesKilled;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    [System.Serializable]
    public class SaveData
    {
        public string _bestPlayerName;
        public int _bestPlayerScore;
        public int _bestPlayerEnemiesKilled;
    }

    public void SaveBestPlayerData(string bestPlayerName, int bestPlayerScore, int bestPlayerEnemiesKilled)
    {
        SaveData data = new SaveData();
        data._bestPlayerName = bestPlayerName;
        data._bestPlayerScore = bestPlayerScore;
        data._bestPlayerEnemiesKilled = bestPlayerEnemiesKilled;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }

    public void LoadBestPlayerData()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayerName = data._bestPlayerName;
            bestPlayerScore = data._bestPlayerScore;
            bestPlayerEnemiesKilled = data._bestPlayerEnemiesKilled;
        }
    }
}
