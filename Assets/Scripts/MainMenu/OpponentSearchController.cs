using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class OpponentSearchController : MonoBehaviour
{

    public InputField usernameInput;
    public Button searchButton;
    public Text notFoundLabel;

    private void Start()
    {
        notFoundLabel.gameObject.SetActive(false);
        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.AuthEstablished) DataManager.Instance.BypassAuth("DEV lucaspopp0@gmail.com");
    }

    public void UsernameChanged()
    {
        searchButton.enabled = UsernameIsValid(usernameInput.text);
        notFoundLabel.gameObject.SetActive(false);
    }

    private bool UsernameIsValid(string username)
    {
        return !Regex.IsMatch(username, "\\W");
    }

    public void Search()
    {
        searchButton.enabled = false;
        notFoundLabel.gameObject.SetActive(false);
        StartCoroutine(DataManager.Instance.SearchUser(usernameInput.text.Trim(), (userExists, userInfo) =>
        {
            if (!userExists)
            {
                notFoundLabel.gameObject.SetActive(true);
                searchButton.enabled = false;
            }
            else
            {
                Debug.Log(userInfo.Email);
            }
        }));
    }

}