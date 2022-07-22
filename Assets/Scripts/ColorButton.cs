using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ColorButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(MainDataManager.Instance.backgroundColor == gameObject.GetComponent<Button>().colors.selectedColor)
        {
            gameObject.GetComponentInChildren<Text>().text = "V";
        } else {
            
            gameObject.GetComponentInChildren<Text>().text = "";
        }
    }

    public void selectColor() {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("ColorButton");
        foreach(GameObject button in buttons) {
            button.gameObject.GetComponentInChildren<Text>().text = "";
        }
        gameObject.GetComponentInChildren<Text>().text = "V";
        MainDataManager.Instance.backgroundColor = gameObject.GetComponent<Button>().colors.selectedColor;
    }
}
