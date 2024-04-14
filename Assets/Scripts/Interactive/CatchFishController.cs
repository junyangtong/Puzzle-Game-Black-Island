using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFishController : Interactive
{
    public GameObject miniGameSceneToGo;
    
    protected override void OnClickedAction()
    {
        miniGameSceneToGo.SetActive(true);
        EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
    }
    public override void EmptyClicked()
    {
        
    }
    public void Close()
    {
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        miniGameSceneToGo.SetActive(false);
    }

}