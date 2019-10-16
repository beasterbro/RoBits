using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class MainMenu : MonoBehaviour
{

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
        DataManager.GetManager().EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.GetManager().FetchInitialData();

        Debug.Log("All Parts:");
        Debug.Log(JsonConvert.SerializeObject(DataManager.GetManager().GetAllParts()));

        Debug.Log("Inventory:");
        Debug.Log(JsonConvert.SerializeObject(DataManager.GetManager().GetUserInventory()));

        Debug.Log("Teams:");
        Debug.Log(JsonConvert.SerializeObject(DataManager.GetManager().GetUserTeams()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
