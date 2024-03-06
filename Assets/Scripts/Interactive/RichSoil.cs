using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichSoil : Interactive
{
    public GameObject tree1;
    public GameObject tree2;
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    protected override void OnClickedAction()
    {
        // 种下种子 显示时间线1 2中的树
        Debug.Log("种下种子");
        tree1.SetActive(true);
        tree2.SetActive(true);
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
