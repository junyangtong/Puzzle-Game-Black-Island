using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[RequireComponent(typeof(DialogueController))]
public class Bonfires : Interactive
{
    public GameObject Fire;
    public PlayableDirector GameOverTemp;
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    protected override void OnClickedAction()
    {
        // 放进树枝 篝火燃烧 游戏结束
        Debug.Log("游戏结束");
        GameOverTemp.Play();
        Fire.SetActive(true);
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
