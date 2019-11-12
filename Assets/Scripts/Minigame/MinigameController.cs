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

        DataManager.Instance.EstablishAuth("lucaspopp0@gmail.com");
        Debug.Log("Auth established.");
        await DataManager.Instance.FetchInitialData();
        Debug.Log("Data loaded.");
        UpdateRemaining();

        Debug.Log(DataManager.Instance.UserTeams[0].Name);

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
                loadingPanel.SetActive(true);
                OnActionsCompleted();
            }
        }
    }

    private async void OnActionsCompleted()
    {
        foreach (TeamInfo team in DataManager.Instance.UserTeams)
        {
            Debug.Log(team.DateLastMaintained);
            team.SetMaintained();
            await DataManager.Instance.UpdateTeam(team);
        }
        Debug.Log("Updated teams.");
        actions = 0;
        UpdateRemaining();
        loadingPanel.SetActive(false);
    }
}
