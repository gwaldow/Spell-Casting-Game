using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void playGame()
    {
        Debug.Log("Play button clicked - main menu");
        SceneManager.LoadScene(1);
    }

    public void showHelp()
    {
        Debug.Log("Help button clicked - main menu");
        transform.Find("PlayButton").gameObject.SetActive(false);
        transform.Find("HelpButton").gameObject.SetActive(false);
        transform.Find("QuitButton").gameObject.SetActive(false);
        transform.Find("TitleText").gameObject.SetActive(false);
        transform.Find("BackButton").gameObject.SetActive(true);
        transform.Find("HelpScreen").gameObject.SetActive(true);
    }

    public void quitGame()
    {
        Debug.Log("Quit button clicked - main menu");
        Application.Quit();
    }

    public void back()
    {
        transform.Find("PlayButton").gameObject.SetActive(true);
        transform.Find("HelpButton").gameObject.SetActive(true);
        transform.Find("TitleText").gameObject.SetActive(true);
        transform.Find("QuitButton").gameObject.SetActive(true);
        transform.Find("BackButton").gameObject.SetActive(false);
        transform.Find("HelpScreen").gameObject.SetActive(false);
    }
}

