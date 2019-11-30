using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{

    private bool userLoggedIn;
    private string googleToken;
    private bool isEstablishingAuth;

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
        ShowLoginButton();
        DataManager.Instance.Latch(this);
    }

    private void Update()
    {
        #if UNITY_EDITOR
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
            StartCoroutine(DataManager.Instance.EstablishAuth(googleToken, success =>
            {
                if (success)
                {
                    SceneManager.LoadScene(Scenes.Menu);
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

}