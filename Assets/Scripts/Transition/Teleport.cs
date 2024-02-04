using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Teleport : MonoBehaviour
{
    public GameObject sceneFrom;

    public GameObject sceneToGo;
    
    private bool canTransition;
    private void Start() {
        canTransition = true;
    }
    private void OnEnable() 
    {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
    }
    private void OnDisable() 
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
    }
    private void OnGameStateChangeEvent(GameState gameState)
    {
        canTransition = gameState == GameState.GamePlay;
    }
    public void TeleportToScene()
    {
        if(canTransition)
        {
            sceneToGo.SetActive(true);
            sceneFrom.SetActive(false);
        }
    }
}
