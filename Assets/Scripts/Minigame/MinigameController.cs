using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Text remainingText;
    [SerializeField] private int actionsNeeded = 5;
    [SerializeField] private string[] actionIds;
    private int actions = 0;

    private async void Start()
    {
        loadingPanel.SetActive(true);

        DataManager.Instance.EstablishAuth("jackson.nelsongal@gmail.com");
        Debug.Log("Auth established.");
        await DataManager.Instance.FetchInitialData();
        Debug.Log("Data loaded.");
        UpdateRemaining();

        loadingPanel.SetActive(false);
    }

    private void UpdateRemaining()
    {
        remainingText.text = "Actions Remaining: " + (actionsNeeded - actions);
    }

    public void PerformAction(string id)
    {
        if (actionIds.Length > 0 && actionIds[actions % actionIds.Length] == id)
        {
            actions++;
            UpdateRemaining();
            if (actions >= actionsNeeded)
            {
                OnActionsCompleted();
            }
        }
    }

    private void OnActionsCompleted()
    {
        foreach (TeamInfo team in DataManager.Instance.UserTeams)
        {
            team.SetMaintained();
            DataManager.Instance.UpdateTeam(team);
        }
    }
}
