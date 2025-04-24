using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverUI;
    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }
    public void RestartGame()
    {
        PlayerItemData.instance.ResetStats();
        SceneManager.LoadScene(1);
    }
    public void ExitToMainMenu()
    {
        FindFirstObjectByType<EvolutionUI>().SaveKnownEvolutions();
        PlayerItemData.instance.ResetStats();
        SceneManager.LoadScene(0);
    }
}
