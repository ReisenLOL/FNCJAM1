using Bremsengine;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUIElements;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GeneralManager.TogglePause();
            pauseUIElements.SetActive(!pauseUIElements.activeSelf);
        }
    }
    public void Resume()
    {
        GeneralManager.SetPause(false);
        pauseUIElements.SetActive(false);
    }
    public void ExitGame()
    {
        Debug.Log("Game Exited");
    }
}
