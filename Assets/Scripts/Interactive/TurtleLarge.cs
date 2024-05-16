using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TurtleLarge : Interactive
{
    public PlayableDirector moveToWater;
    public PlayableDirector moveToIsland;
    public GameObject H1Player;
    public GameObject TelePort;
    public GameObject Bag;
    public bool inWater = false;
    
    private void Awake()
    {

    }
    
    protected override void OnClickedAction()
    {

    }
    public override void EmptyClicked()
    {
        if(!inWater)
            MoveToWater();
        else
            MoveToIsland();
    }
    private void MoveToWater()
    {
        moveToWater.Play();
    }
    private void MoveToIsland()
    {
        moveToIsland.Play();
        // 隐藏主角
        H1Player.SetActive(false);
        // 隐藏UI
        TelePort.SetActive(false);
        Bag.SetActive(false);
    }
}
