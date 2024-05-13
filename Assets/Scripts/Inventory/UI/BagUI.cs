using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    public bool CursorInBagUI = false;
    private Animation anim;
    private bool canOpenBag;
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
        canOpenBag = gameState == GameState.GamePlay;
    }
    private void Start() 
    {
        anim = GetComponent<Animation>();
    }
    public void BagClose()
    {
        anim.Play("BagClose");
    }
    public void BagOpen()
    {
        if(canOpenBag)
            anim.Play("BagOpen");
    }
    public void CursorHover()
    {
        CursorInBagUI = true;
    }
    public void CursorExit()
    {
        CursorInBagUI = false;
    }
}
