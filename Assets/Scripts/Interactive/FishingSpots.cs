using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpots : Interactive
{
    public GameObject Fish;
    private DialogueController dialogueController;
    
    private void Awake()
    {
        Fish.SetActive(false);
        dialogueController = GetComponent<DialogueController>();
    }
    
    protected override void OnClickedAction()
    {
        // 播放使用物品的动画
        Debug.Log("开始钓鱼");
        // 出现 鱼
        Fish.SetActive(true);
        dialogueController.ShowdialogueFinish();
    }
    public override void EmptyClicked()
    {
        if(!isDone)
            dialogueController.ShowdialogueFinish();
        else
            dialogueController.ShowdialogueEmpty();
    }
}
