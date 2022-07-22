using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverPanel;

    private bool m_Started = false;
    private int m_Points;
    public Text PlayerText;
    private GameObject MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        PlayerText.text = "Player: " + MainDataManager.Instance.currentPlayer;
        SetBestScoreText(GetBestScore());

        SetGameBackground();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
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
    public void GameOver()
    {
        MainDataManager.Instance.currentPlayerScore = m_Points;
        GameOverPanel.SetActive(true);
        SaveHighScore();
        StartCoroutine("GoToHighScores");
    }

    public void SaveHighScore()
    {
        string path = Application.persistentDataPath + "/topPlayers.json";
        SaveData dataToSave = new SaveData();
        if (File.Exists(path))
        {
            string jsonRead = File.ReadAllText(path);
            SaveData dataSaved = JsonUtility.FromJson<SaveData>(jsonRead);
            PlayerScore playerScore = new PlayerScore();
            playerScore.playerName = MainDataManager.Instance.currentPlayer;
            playerScore.score = m_Points;
            List<PlayerScore> newTopPlayers = new List<PlayerScore>();
            if (dataSaved != null && dataSaved.topPlayers != null && dataSaved.topPlayers.Count > 0)
            {
                dataSaved.topPlayers.Add(playerScore);
                dataSaved.topPlayers.Sort((p1, p2) => -p1.score.CompareTo(p2.score));
                if (dataSaved.topPlayers.Count > 3)
                {
                    dataSaved.topPlayers.Remove(dataSaved.topPlayers[dataSaved.topPlayers.Count - 1]);
                }
                dataToSave.topPlayers = dataSaved.topPlayers;
            }
            else
            {
                dataToSave.topPlayers = new List<PlayerScore>();
                playerScore.playerName = MainDataManager.Instance.currentPlayer;
                playerScore.score = m_Points;
                dataToSave.topPlayers.Add(playerScore);
            }
        }
        else
        {
            dataToSave.topPlayers = new List<PlayerScore>();
            PlayerScore playerScore = new PlayerScore();
            playerScore.playerName = MainDataManager.Instance.currentPlayer;
            playerScore.score = m_Points;
            dataToSave.topPlayers.Add(playerScore);
        }

        SetBestScoreText(dataToSave.topPlayers[0]);

        string json = JsonUtility.ToJson(dataToSave);

        File.WriteAllText(Application.persistentDataPath + "/topPlayers.json", json);
    }

    private void SetBestScoreText(PlayerScore playerScore)
    {
        BestScoreText.text = "Best score\n" + playerScore.playerName + ": " + playerScore.score;
    }

    private PlayerScore GetBestScore()
    {
        string path = Application.persistentDataPath + "/topPlayers.json";
        PlayerScore playerScore = new PlayerScore();
        if (File.Exists(path))
        {
            string jsonRead = File.ReadAllText(path);
            SaveData dataSaved = JsonUtility.FromJson<SaveData>(jsonRead);
            if (dataSaved != null && dataSaved.topPlayers != null && dataSaved.topPlayers.Count > 0)
            {
                playerScore = dataSaved.topPlayers[0];
            }
            else
            {
                playerScore.playerName = "";
                playerScore.score = 0;
            }
        }
        else
        {
            playerScore.playerName = "";
            playerScore.score = 0;
        }
        return playerScore;
    }

    IEnumerator GoToHighScores()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }

    private void SetGameBackground()
    {
        MainCamera = GameObject.Find("Main Camera");
        Camera camera = MainCamera.GetComponent<Camera>();
        camera.backgroundColor = MainDataManager.Instance.backgroundColor;
    }
}
