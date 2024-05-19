using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleInP1 : MonoBehaviour
{
    private void OnEnable() 
    {
        EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
    }

    private void OnDisable() 
    {
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
    }
    
}
