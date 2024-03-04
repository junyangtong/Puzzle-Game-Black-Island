using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBoat : Interactive
{
    public GameObject miniGameSceneToGo;

    protected override void OnClickedAction()
    {
        miniGameSceneToGo.SetActive(true);
        EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
    }
    public void Close()
    {
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        miniGameSceneToGo.SetActive(false);
    }
}
