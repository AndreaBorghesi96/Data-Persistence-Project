using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDataManager : MonoBehaviour
{
    public static MainDataManager Instance;
    public int bestScore = 0;
    public string bestPlayer = "";
    public string currentPlayer = "";
    public int currentPlayerScore = 0;
    public Color backgroundColor;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

}
