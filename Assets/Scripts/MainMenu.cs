using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //Changes the scene to the battle scene
    public void PlayGame()
    {
        SceneManager.LoadScene("Simulation");
    }

    public void LoadBehaviorLab()
    {
        SceneManager.LoadScene("BehaviorLab");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.AuthEstablished) DataManager.Instance.BypassAuth("DEV lucaspopp0@gmail.com");
        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary());
    }

}