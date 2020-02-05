using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update

    public void PlayGame()
    {
        SceneManager.LoadScene("DungeonGeneration",LoadSceneMode.Single);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartGUI",LoadSceneMode.Single);
    }



}
