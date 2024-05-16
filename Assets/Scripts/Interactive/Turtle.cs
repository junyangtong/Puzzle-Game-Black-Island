using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(DialogueController))]
public class Turtle : Interactive
{
    public GameObject TurtleToolTip;
    public GameObject NPCDialogue;
    public GameObject MainDialogue;
    public GameObject Choose1UI;
    public GameObject Choose2UI;
    public GameObject Choose3UI;
    public GameObject TurtlePickUp;
    public PlayableDirector playableDirector;
    private bool beforeChoose1 = true;
    private bool beforeChoose2 = false;
    private bool beforeChoose3 = false;
    public DialogueData_SO dialogue3;

    private DialogueController dialogueController;
    private void Awake()
    {
        NPCDialogue.SetActive(false);
        dialogueController = GetComponent<DialogueController>();
    }
       // dialogueController.ShowdialogueFinish();
    private void Update() 
    {
        if (dialogueController.talkOver && beforeChoose1)
        {
            Choose1UI.SetActive(true);
            EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
            dialogueController.talkOver = false;
        }
        if (dialogueController.talkOver && beforeChoose2)
        {
            Choose2UI.SetActive(true);
            EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
            dialogueController.dialogueFinish = dialogue3;
            dialogueController.FilldialogueStack();
            dialogueController.talkOver = false;
        }
        if (dialogueController.talkOver && beforeChoose3)
        {
            Choose3UI.SetActive(true);
            EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
            dialogueController.talkOver = false;
        }
    }
    public override void EmptyClicked()
    {
        TurtleToolTip.SetActive(false);
        if(isDone)
            dialogueController.ShowdialogueFinish();
        else
            dialogueController.ShowdialogueEmpty();
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Player")
        {
            //Debug.Log("主角进入范围");
            MainDialogue.SetActive(false);
            NPCDialogue.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Player")
        {
            // 离开可交互物品时取消对话框并 恢复为游戏进行状态
            EventHandler.CallShowDialogueEvent(string.Empty);
            MainDialogue.SetActive(true);
            NPCDialogue.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (MainDialogue != null)
        {
            MainDialogue.SetActive(true);
            NPCDialogue.SetActive(false);
        }
        
    }
    public void PlayStory()
    {
        // 播放连环画
        playableDirector.Play();
        Choose1UI.SetActive(false);
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        beforeChoose1 = false;
        beforeChoose2 = true;
        isDone = true;
    }
    public void CloseChoose1()
    {
        Choose1UI.SetActive(false);
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
    }
    public void CloseChoose3()
    {
        Choose3UI.SetActive(false);
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
    }
    public void CloseChoose2()
    {
        Choose2UI.SetActive(false);
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
    }
    public void TeleportHelp()
    {
        Choose2UI.SetActive(true);
    }
    public void SwitchTurtle()
    {
        TurtlePickUp.SetActive(true);
        Choose3UI.SetActive(false);
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        this.gameObject.SetActive(false);
    }
    
    public void Understood()
    {
        Choose2UI.SetActive(false);
        beforeChoose2 = false;
        beforeChoose3 = true;
    }
    
    public void WatchAgain()
    {
        Choose2UI.SetActive(false);
    }
}
