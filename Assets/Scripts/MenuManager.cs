using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

[DefaultExecutionOrder(1000)]
public class MenuManager : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private Button startButton;

    void Start()
    {
        SetBackgroundColor();
    }

    private void Update()
    {
        string trimmedName = playerName.text.Trim();
        if (trimmedName.Length < 1 || trimmedName.Length > 8)
        {
            startButton.enabled = false;
        }
        else
        {
            startButton.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Return) && startButton.enabled)
        {
            playerName.text = trimmedName;
            Play();
        }
    }
    public void Play()
    {
        SetCurrentPlayerName(playerName.text);

        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene(3);
    }

    private void SetCurrentPlayerName(string name)
    {
        MainDataManager.Instance.currentPlayer = name;
    }

    [System.Serializable]
    class Settings
    {
        public Color backgroundColor;
    }
    private void SetBackgroundColor()
    {
        string path = Application.persistentDataPath + "/settings.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Settings settings = JsonUtility.FromJson<Settings>(json);
            MainDataManager.Instance.backgroundColor = settings.backgroundColor;
        }
        else
        {
            MainDataManager.Instance.backgroundColor = Color.black;
        }
    }
}
