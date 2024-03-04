using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class FishingMan : Interactive
{
    public GameObject FishingRod;
    private DialogueController dialogueController;
    
    private void Awake()
    {
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
}