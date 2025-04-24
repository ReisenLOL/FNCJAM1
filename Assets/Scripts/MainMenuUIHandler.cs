using Core.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour
{
    public GameObject CreditsUI;
    public GameObject OptionsUI;
    public GameObject MusicRoomUI;
    public GameObject MainMenuUI;
    public AudioSource audioSource;
    #region Music Stuff
    [System.Serializable]
    public class Music
    {
        public AudioClip sound;
        public string description;
    }
    public List<Music> songList;
    public Transform songListUI;
    public TextMeshProUGUI songTitleUI;
    public TextMeshProUGUI songDescriptionUI;
    public Button MusicButton;
    public AudioClip currentlyPlaying;
    public void AddMusic(Music music)
    {
        Button newButton = Instantiate(MusicButton, songListUI);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = music.sound.name;
        newButton.onClick.AddListener(() => PlayMusic(music));
        newButton.gameObject.SetActive(true);
    }
    public void PlayMusic(Music music)
    {
        songTitleUI.text = music.sound.name;
        songDescriptionUI.text = music.description;
        currentlyPlaying = music.sound;
        audioSource.PlayOneShot(currentlyPlaying);
    }
    #endregion
    private void Start()
    {
        for (int i = 0; i < songList.Count; i++)
        {
            AddMusic(songList[i]);
        }
    }
    //funny function spam
    public void StartGame()
    {
        Timer.instance.ResetTimer();
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
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
