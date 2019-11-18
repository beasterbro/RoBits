using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

public class MainMenu : MonoBehaviour
{

    //Changes the scene to the battle scene
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadBehaviorLab()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.Latch(this);
        DataManager.Instance.EstablishAuth("lucaspopp0@gmail.com");
        StartCoroutine(DataManager.Instance.FetchInitialData(() =>
        {
            Debug.Log("All Parts:");
            Debug.Log(JsonConvert.SerializeObject(DataManager.Instance.AllParts));

            Debug.Log("Inventory:");
            Debug.Log(JsonConvert.SerializeObject(DataManager.Instance.UserInventory));

            Debug.Log("Teams:");
            Debug.Log(JsonConvert.SerializeObject(DataManager.Instance.UserTeams));
        }));
    }

}