using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    public GameObject CreditsUI;
    public GameObject OptionsUI;
    public GameObject MusicRoomUI;
    public GameObject MainMenuUI;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ShowCredits()
    {
        CreditsUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }
    public void HideCredits()
    {
        CreditsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }
    public void ShowOptions()
    {
        OptionsUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }
    public void HideOptions()
    {
        OptionsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }
    public void ShowMusicRoom()
    {
        MusicRoomUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }
    public void HideMusicRoom()
    {
        MusicRoomUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }
    //funny function spam
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
