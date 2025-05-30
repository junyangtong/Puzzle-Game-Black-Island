using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class OutDoorProtectionScop : MonoBehaviour
{
    private DialogueController dialogueController;
    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // 角色自言自语
        Debug.Log("自言自语");
        var num = Random.Range(0, 2);
        if(num == 0)
            dialogueController.ShowdialogueEmpty();
        else
            dialogueController.ShowdialogueFinish();

        // 一段时间后对话框消失
        Invoke("CleanDialogue", 2f);
    }
    private void CleanDialogue()
    {
        EventHandler.CallShowDialogueEvent(string.Empty);
    }
}
