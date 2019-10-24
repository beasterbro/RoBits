using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotStatusDisplay : MonoBehaviour
{

    public BotController bot;
    public GameObject previewPanel;
    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = bot.currentHealth / bot.MAX_HEALTH;
    }
}
