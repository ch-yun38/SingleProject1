using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerProj : MonoBehaviour
{
    public GameObject startUI;
    public GameObject clearUI;
    public GameObject deadUI;
    public int killCount;
    public int goal = 10;


    public void StartGame()
    {
        Time.timeScale = 1;
        startUI.SetActive(false);
        deadUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void AddKill()
    {
        killCount++;
        if (killCount >= goal)
        {
            GameClear();
        }
    }
    public void GameClear()
    {
        Time.timeScale = 0;
        clearUI.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        deadUI.SetActive(true);
    }
    public void ReturnTitle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }




    void Start()
    {
        Time.timeScale = 0;
        startUI.SetActive(true);
        clearUI.SetActive(false);
        deadUI.SetActive(false);


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

 
}
