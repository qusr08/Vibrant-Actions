using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour{
    
    //Load game scene when play button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    } 

    public void QuitGame()
    { 
        Debug.Log("Quit Game");
        //This will only work for the build, not within unity
        Application.Quit();
    } 
}