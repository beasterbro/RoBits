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

    private void Start()
    {
        loadingPanel.SetActive(true);

        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.AuthEstablished) DataManager.Instance.BypassAuth("DEV lucaspopp0@gmail.com");
        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary(success =>
        {
            loadingPanel.SetActive(false);

            if (!success) Debug.Log("Failed to load data.");
            else
            {
                Debug.Log("Data loaded.");
                UpdateRemaining();
            }
        }));
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

    private void OnActionsCompleted()
    {
        var updateStatuses = new Dictionary<TeamInfo, bool>();
        var anyFailed = false;

        foreach (var team in DataManager.Instance.UserTeams)
        {
            team.SetMaintained();
            updateStatuses[team] = false;

            StartCoroutine(DataManager.Instance.UpdateTeam(team, updateSucceeded =>
            {
                updateStatuses[team] = updateSucceeded;

                if (anyFailed) return;

                if (!updateSucceeded)
                {
                    updateStatuses[team] = false;
                    anyFailed = true;
                    Debug.LogWarning("Failed to update team: " + team.Name);
                    actions = 0;
                    UpdateRemaining();
                    loadingPanel.SetActive(false);
                }
                else if (!updateStatuses.ContainsValue(false))
                {
                    Debug.Log("Updated teams.");
                    actions = 0;
                    UpdateRemaining();
                    loadingPanel.SetActive(false);
                }
            }));
        }
    }

}