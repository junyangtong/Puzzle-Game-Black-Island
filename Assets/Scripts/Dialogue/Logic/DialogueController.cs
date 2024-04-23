using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO dialogueEmpty;
    public DialogueData_SO dialogueFinish;
    private Stack<string> dialogueEmptyStack;
    private Stack<string> dialogueFinishStack;
    private bool isTalking;


    private void Awake() 
    {
        FilldialogueStack();
    }
    //逐一输出对话
    private void FilldialogueStack()
    {
        dialogueEmptyStack = new Stack<string>();
        dialogueFinishStack = new Stack<string>();

        for(int i = dialogueEmpty.dialogueList.Count -1;i>-1;i--)
        {
            dialogueEmptyStack.Push(dialogueEmpty.dialogueList[i]);
        }
        for(int i = dialogueFinish.dialogueList.Count -1;i>-1;i--)
        {
            dialogueFinishStack.Push(dialogueFinish.dialogueList[i]);
        }
    }

    public void ShowdialogueEmpty()
    {
        if(!isTalking)
            StartCoroutine(DialogueRoutine(dialogueEmptyStack));
    }
    public void ShowdialogueFinish()
    { 
        if(!isTalking)
            StartCoroutine(DialogueRoutine(dialogueFinishStack));
    }

    private IEnumerator DialogueRoutine(Stack<string> data)
    {
        isTalking = true;
        if(data.TryPop(out string result))
        {
            EventHandler.CallShowDialogueEvent(result);
            yield return null;
            isTalking = false;
            //EventHandler.CallGameStateChangeEvent(GameState.Pause);
        }
        else
        {
            EventHandler.CallShowDialogueEvent(string.Empty);
            FilldialogueStack();
            isTalking = false;
            //EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        }
    }
}
