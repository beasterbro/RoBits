using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //Changes the scene to the battle scene
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadBehaviorLab()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }


    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.Latch(this);
        if (!DataManager.Instance.InitialFetchPerformed) DataManager.Instance.BypassAuth("DEV lucaspopp0@gmail.com");
        StartCoroutine(DataManager.Instance.FetchInitialDataIfNecessary());
    }

}