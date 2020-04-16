using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUi : MonoBehaviour
{
    [SerializeField] private GameStarter gameStarter;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField familyNameInput;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RandomizeName()
    {
        nameInput.text = NameHelper.GenerateName();
    }

    public void RandomizeFamilyName()
    {
        familyNameInput.text = NameHelper.GenerateFamilyName();
    }

    public void ApplySetup()
    {
        var playerName = string.IsNullOrWhiteSpace(nameInput.text)
            ? NameHelper.GenerateName()
            : nameInput.text;

        var familyName = string.IsNullOrWhiteSpace(familyNameInput.text)
            ? NameHelper.GenerateFamilyName()
            : familyNameInput.text;

        gameStarter.StartGame(playerName, familyName);
    }

    public void ResetCamPos()
    {
        Camera.main.transform.position = new Vector3(6.85f, 0, -10);
    }
}
