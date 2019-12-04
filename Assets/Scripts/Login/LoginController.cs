using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{

    public GameObject userCreationPanel;
    public InputField usernameInput;
    public Text errorLabel;

    private bool userLoggedIn;
    private string googleToken;
    private bool isEstablishingAuth;

    private string newUserEmail;
    
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void ShowLoginButton();
    #else
    private static void ShowLoginButton() { }
    #endif

    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void HideLoginButton();
    #else
    private static void HideLoginButton() { }
    #endif

    public void OnSignIn(string token)
    {
        userLoggedIn = true;
        googleToken = token;
    }

    private void Start()
    {
        userCreationPanel.SetActive(false);
        ShowLoginButton();
        DataManager.Instance.Latch(this);
    }

    private void Update()
    {
        #if false && UNITY_EDITOR
        if (!isEstablishingAuth)
        {
            isEstablishingAuth = true;
            DataManager.Instance.BypassAuth("DEV lucaspopp0@gmail.com");
            SceneManager.LoadScene(Scenes.Menu);
        }
        #else
        if (userLoggedIn && googleToken != null && !isEstablishingAuth && !DataManager.Instance.AuthEstablished)
        {
            HideLoginButton();
            isEstablishingAuth = true;
            
            StartCoroutine(DataManager.Instance.EstablishAuth(googleToken, (success, email) =>
            {
                if (success)
                {
                    StartCoroutine(DataManager.Instance.EmailIsTaken(email, isExistingUser =>
                    {
                        if (isExistingUser) SceneManager.LoadScene(Scenes.Menu);
                        else
                        {
                            newUserEmail = email;
                            errorLabel.gameObject.SetActive(false);
                            userCreationPanel.SetActive(true);
                        }
                    }));
                }
                else
                {
                    isEstablishingAuth = false;
                    userLoggedIn = false;
                    googleToken = null;
                    ShowLoginButton();
                }
            }));
        }
        #endif
    }

    public void AttemptJoin()
    {
        var username = usernameInput.text.Trim();

        if (username.Length == 0 || Regex.IsMatch(username, "\\W"))
        {
            errorLabel.gameObject.SetActive(true);
            errorLabel.text = "Invalid username";
        }
        else
        {
            errorLabel.gameObject.SetActive(false);
            StartCoroutine(DataManager.Instance.UsernameIsTaken(username, isTaken =>
            {
                if (isTaken)
                {
                    errorLabel.gameObject.SetActive(true);
                    errorLabel.text = "Username taken";
                }
                else
                {
                    StartCoroutine(DataManager.Instance.CreateUser(username, newUserEmail, success =>
                    {
                        if (!success) return;
                        SceneManager.LoadScene(Scenes.Menu);
                    }));
                }
            }));
        }
    }

}