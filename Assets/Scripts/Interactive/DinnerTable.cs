using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinnerTable : Interactive
{
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    
    protected override void OnClickedAction()
    {
        // 播放使用物品的动画
        Debug.Log("鱼放在海鸭的餐桌");

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
