using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class OpponentSearchController : MonoBehaviour
{

    [SerializeField] private Text usernameInput;
    [SerializeField] private Button searchButton;
    [SerializeField] private Text resultText;
    [SerializeField] private Button previewTeamsButton;
    [SerializeField] private GameObject searchMenu;
    [SerializeField] private SelectOpposingMenu selectOpposingMenu;

    private UserInfo opponent;
    private void Start()
    {
        resultText.gameObject.SetActive(false);
        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.AuthEstablished) DataManager.Instance.BypassAuth("DEV lucaspopp0@gmail.com");
    }

    public void UsernameChanged()
    {
        searchButton.gameObject.SetActive(IsUsernameValid(usernameInput.text));
        resultText.gameObject.SetActive(false);
    }

    private bool IsUsernameValid(string username)
    {
        return !Regex.IsMatch(username, "\\W");
    }

    private void ShowResult(bool result)
    {
        if (result)
        {
            resultText.text = "Opponent found!";
        }
        else
        {
            resultText.text = "Opponent not found :(";
        }
        
        resultText.gameObject.SetActive(true);
    }

    private void HideResult()
    {
        resultText.gameObject.SetActive(false);
    }
    
    public void Search()
    {
        searchButton.gameObject.SetActive(false);
        HideResult();
        StartCoroutine(DataManager.Instance.SearchUser(usernameInput.text.Trim(), (userExists, userInfo) =>
        {
            if (!userExists)
            {
                opponent = null;
                ShowResult(false);
                searchButton.gameObject.SetActive(false);
                var searchButtonSpriteState = searchButton.spriteState;
                searchButtonSpriteState.pressedSprite = searchButton.spriteState.highlightedSprite;
            }
            else
            {
                opponent = userInfo;
                ShowResult(true);
                
                Debug.Log(userInfo.Email);
                previewTeamsButton.gameObject.SetActive(true);
            }
            
            searchButton.gameObject.SetActive(true);
        }));
    }

    public void ShowEnemyTeamSelection()
    {
        selectOpposingMenu.enemy = opponent;
        searchMenu.SetActive(false);
        selectOpposingMenu.gameObject.SetActive(true);
    }

}