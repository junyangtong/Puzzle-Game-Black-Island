using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class TurtlePutPlace : Interactive
{
    public GameObject Turtle;
    public GameObject TurtleMiddle;
    public GameObject TurtleLarge;
    public GameObject Mark;
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    protected override void OnClickedAction()
    {
        Turtle.SetActive(true);
        TurtleMiddle.SetActive(true);
        TurtleLarge.SetActive(true);
        Mark.SetActive(false);
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
