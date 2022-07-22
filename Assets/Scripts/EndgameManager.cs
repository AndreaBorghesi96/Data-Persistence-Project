using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class EndgameManager : MonoBehaviour
{
    [SerializeField] private Text YourScore;
    [SerializeField] private Text TopCutScores;

    // Start is called before the first frame update
    void Start()
    {
        YourScore.text = "Your score: " + MainDataManager.Instance.currentPlayerScore;
        TopCutScores.text = "Top 3 players:\n";
        string path = Application.persistentDataPath + "/topPlayers.json";
        if (File.Exists(path))
        {
            string jsonRead = File.ReadAllText(path);
            SaveData dataSaved = JsonUtility.FromJson<SaveData>(jsonRead);
            if (dataSaved != null && dataSaved.topPlayers != null && dataSaved.topPlayers.Count > 0)
            {
                foreach (PlayerScore score in dataSaved.topPlayers)
                {
                    TopCutScores.text += score.playerName + " - " + score.score + "\n";
                }
            }
            else
            {
                TopCutScores.text = "Top 3 players:\n" + MainDataManager.Instance.currentPlayer
                + " - " + MainDataManager.Instance.currentPlayerScore;
            }
        }
        else
        {
            TopCutScores.text = "Top 3 players:\n" + MainDataManager.Instance.currentPlayer
            + " - " + MainDataManager.Instance.currentPlayerScore;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public List<PlayerScore> topPlayers;
    }

    [System.Serializable]
    class PlayerScore
    {
        public string playerName;
        public int score;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
