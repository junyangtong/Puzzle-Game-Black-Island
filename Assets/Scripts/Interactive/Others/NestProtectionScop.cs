using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class NestProtectionScop : MonoBehaviour
{
    private DialogueController dialogueController;
    public Animator anim;
    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // 角色自言自语
            Debug.Log("自言自语");
            anim.SetTrigger("angry");
            var num = Random.Range(0, 2);
            if(num == 0)
                dialogueController.ShowdialogueEmpty();
            else
                dialogueController.ShowdialogueFinish();

            // 一段时间后对话框消失
            Invoke("CleanDialogue", 2f);
        }
    }
    private void CleanDialogue()
    {
        EventHandler.CallShowDialogueEvent(string.Empty);
    }
}
