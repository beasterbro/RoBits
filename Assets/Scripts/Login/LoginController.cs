using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{

    private bool userLoggedIn;
    private string googleToken;

    public void OnSignIn(string token)
    {
        userLoggedIn = true;
        googleToken = token;
    }

    private void Start()
    {
        DataManager.Instance.Latch(this);
    }

    private void Update()
    {
        if (userLoggedIn && googleToken != null)
        {
            StartCoroutine(DataManager.Instance.EstablishAuth(googleToken, success =>
            {
                if (!success) return;
                SceneManager.LoadScene(0);
            }));
        }
    }

}
