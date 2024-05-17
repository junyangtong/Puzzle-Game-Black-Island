using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class TalkSelf : MonoBehaviour
{
    private DialogueController dialogueController;
    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    private void OnEnable()
    {
        // 角色自言自语
        dialogueController.ShowdialogueEmpty();
    }
    private void OnDisable() 
    {
        CleanDialogue();
    }
    private void CleanDialogue()
    {
        EventHandler.CallShowDialogueEvent(string.Empty);
    }
}
