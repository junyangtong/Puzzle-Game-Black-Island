using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class FishingMan : Interactive
{
    public GameObject FishingRod;
    public GameObject NPCDialogue;
    public GameObject MainDialogue;
    private DialogueController dialogueController;
    
    private void Awake()
    {
        NPCDialogue.SetActive(false);
        FishingRod.SetActive(false);
        dialogueController = GetComponent<DialogueController>();
    }
    
    protected override void OnClickedAction()
    {
        // 钓鱼人交出鱼竿
        FishingRod.SetActive(true);
        dialogueController.ShowdialogueFinish();
    }
    public override void EmptyClicked()
    {
        if(isDone)
            dialogueController.ShowdialogueFinish();
        else
            dialogueController.ShowdialogueEmpty();
    }
    private void OnTriggerEnter(Collider other) 
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
            MainDialogue.SetActive(true);
            NPCDialogue.SetActive(false);
        }
    }
}