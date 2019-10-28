using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotStatusDisplay : MonoBehaviour
{

    static Color HEALTHY_COLOR = Color.green;
    static Color DEAD_COLOR = Color.red;

    private BotController bot;
    public GameObject previewPanel;
    public Slider healthBar;
    public Image healthImage;

    private GameObject previewBot;

    void Update()
    {
        healthBar.value = bot.currentHealth / bot.maxHealth;
        healthImage.color = Color.Lerp(DEAD_COLOR, HEALTHY_COLOR, healthBar.value);
    }

    public void LoadBot(BotController bot)
    {
        this.bot = bot;
        previewBot = bot.BuildPreview();

        previewBot.transform.parent = previewPanel.transform;
        previewBot.transform.localPosition = Vector3.zero;
        previewBot.transform.localRotation = bot.gameObject.transform.rotation;
        previewBot.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }

}