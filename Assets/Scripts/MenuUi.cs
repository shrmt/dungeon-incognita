using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUi : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenCreatorUrl()
    {
        Application.OpenURL("https://twitter.com/rand_villain");
    }

    public void OpenArtistUrl()
    {
        Application.OpenURL("https://instagram.com/ariendx");
    }

    public void OpenGameUrl()
    {
        Application.OpenURL("https://random-villain.itch.io");
    }
}
