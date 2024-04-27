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
        //FishingRod.SetActive(false);
        dialogueController = GetComponent<DialogueController>();
    }
    
    protected override void OnClickedAction()
    {
        // 钓鱼人交出鱼竿
        FishingRod.SetActive(false);
        InventoryManager.Instance.AddItem(ItemName.FishingRods);
        dialogueController.ShowdialogueFinish();
    }
    public override void EmptyClicked()
    {
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
}