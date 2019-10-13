﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        DataManager.shared.EstablishAuth("lucaspopp0@gmail.com");
        await DataManager.shared.FetchInitialData();
        Debug.Log(DataManager.shared.GetCurrentUserID());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
