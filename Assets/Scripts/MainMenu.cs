using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class MainMenu : MonoBehaviour
{

    //Changes the scene to the battle scene
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


    // Start is called before the first frame update
    async void Start()
    {
        DataManager.Instance().EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.Instance().FetchInitialData();

        Debug.Log("All Parts:");
        Debug.Log(JsonConvert.SerializeObject(DataManager.Instance().GetAllParts()));

        Debug.Log("Inventory:");
        Debug.Log(JsonConvert.SerializeObject(DataManager.Instance().GetUserInventory()));

        Debug.Log("Teams:");
        Debug.Log(JsonConvert.SerializeObject(DataManager.Instance().GetUserTeams()));
    }

}
