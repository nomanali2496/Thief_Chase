using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameRun : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("City 3");
        // Replace "GameScene" with your real scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
