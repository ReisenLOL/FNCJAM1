using Bremsengine;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        FindFirstObjectByType<EvolutionUI>().SaveKnownEvolutions();
        PlayerItemData.instance.ResetStats();
        GeneralManager.TogglePause();
        SceneManager.LoadScene(0);
    }
}
