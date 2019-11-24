using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotStatusDisplay : MonoBehaviour
{

    private static readonly Color HealthyColor = Color.green;
    private static readonly Color DeadColor = Color.red;

    private BotController bot;
    public GameObject previewPanel;
    public Slider healthBar;
    public Image healthImage;

    private GameObject previewBot;

    void Update()
    {
        healthBar.value = bot.currentHealth;
        healthImage.color = Color.Lerp(DeadColor, HealthyColor, healthBar.normalizedValue);
    }

    public void LoadBot(BotController bot)
    {
        this.bot = bot;

        healthBar.maxValue = bot.maxHealth;
        healthBar.value = bot.maxHealth;

        previewBot = bot.BuildPreview();
        previewBot.transform.parent = previewPanel.transform;
        previewBot.transform.localPosition = Vector3.zero;
        previewBot.transform.localRotation = bot.gameObject.transform.rotation;
        previewBot.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }

}