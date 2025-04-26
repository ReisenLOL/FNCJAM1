using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI storyText;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject exitButton;
    [SerializeField] string[] storyString;
    private int currentPage;
    void Start()
    {
        storyText.text = storyString[0];
    }
    public void NextPage()
    {
        currentPage++;
        storyText.text = storyString[currentPage];
        if (currentPage == storyString.Length - 1)
        {
            nextButton.SetActive(false);
            exitButton.SetActive(true);
        }
        if (currentPage > 0)
        {
            backButton.SetActive(true);
        }
    }
    public void PrevPage()
    {
        currentPage--;
        storyText.text = storyString[currentPage];
        if (currentPage == 0)
        {
            backButton.SetActive(false);
        }
        if (currentPage < storyString.Length - 1)
        {
            nextButton.SetActive(true);
            exitButton.SetActive(false);
        }
    }
    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}