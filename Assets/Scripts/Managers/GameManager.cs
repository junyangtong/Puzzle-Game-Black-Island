using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseUI;
    void Start()
    {
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
    }
    void Update() 
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        EventHandler.CallGameStateChangeEvent(GameState.Pause);
    }
    public void ContinueGame()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
    }
    
}
