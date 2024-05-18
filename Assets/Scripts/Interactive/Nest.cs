using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class Nest : Interactive
{
    public GameObject Bird;
    public GameObject BrokenEgg;
    private CapsuleCollider coll;
    private DialogueController dialogueController;
    public bool haveDuck = true;
    public GameObject protectionScope;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
        coll = GetComponent<CapsuleCollider>();
    }
    
    protected override void OnClickedAction()
    {
        // 播放使用物品的动画
        Debug.Log("鸟蛋放在鸟窝里");
        
        // 场景2小鸟出现
        Bird.SetActive(true);
        BrokenEgg.SetActive(true);

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