using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SettingsManager : MonoBehaviour
{

    public void GoToMainMenu()
    {
        SaveBackgroundColor();
        SceneManager.LoadScene(0);
    }

    [System.Serializable]
    class Settings
    {
        public Color backgroundColor;
    }
    private void SaveBackgroundColor()
    {
        string path = Application.persistentDataPath + "/settings.json";
        Settings settings = new Settings();
        settings.backgroundColor = MainDataManager.Instance.backgroundColor;
        string json = JsonUtility.ToJson(settings);

        File.WriteAllText(Application.persistentDataPath + "/settings.json", json);
    }
}
