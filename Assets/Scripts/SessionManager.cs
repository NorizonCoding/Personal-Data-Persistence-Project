using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;

    [System.Serializable]
    public class PlayerScoreData
    {
        public string playerName;
        public int score;

        public PlayerScoreData(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }

        
        public override string ToString()
        {
            return playerName + " : " + score;
        }
    }

    public class PlayerScoreDataComparator : IComparer<PlayerScoreData>
    {
        public int Compare(PlayerScoreData x, PlayerScoreData y)
        {
            return y.score - x.score;
        }
    }

    public string curPlayerName;
    public PlayerScoreData highestPlayer;

    public List<PlayerScoreData> playerScores;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance.playerScores = new List<PlayerScoreData>();
            DeserializeDict();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Exit()
    {
        SerializeDict();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    void SerializeDict()
    {
        string dataPath = Application.persistentDataPath + "/scores.json";
        List<string> playerScoresStr = new();
        foreach (PlayerScoreData data in instance.playerScores)
        {
            playerScoresStr.Add(JsonUtility.ToJson(data));
        }
        File.WriteAllLines(dataPath, playerScoresStr.ToArray());
    }

    public void DeserializeDict()
    {
        string dataPath = Application.persistentDataPath + "/scores.json";

        if (File.Exists(dataPath)) {
            string[] playerScoresStr = File.ReadAllLines(dataPath);

            foreach (string data in playerScoresStr)
            {
                PlayerScoreData item = JsonUtility.FromJson<PlayerScoreData>(data);
                instance.playerScores.Add(item);
            }
        }
    }

    public string ListToStr()
    {
        if (instance.playerScores.Count == 0)
        {
            print("Empty");
            return "";
        }
        instance.playerScores.Sort(new PlayerScoreDataComparator());
        string str = "";

        foreach (PlayerScoreData item in instance.playerScores)
        {
            str += item.ToString() + "\n";
        }
        return str;
    }
}
