using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneScript : MonoBehaviour
{
    public void OnRestartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}