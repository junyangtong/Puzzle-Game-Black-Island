using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
public class DinnerTable : Interactive
{
    public PlayableDirector playableDirector;
    private DialogueController dialogueController;

    private void Awake()
    {
        dialogueController = GetComponent<DialogueController>();
    }
    
    protected override void OnClickedAction()
    {
        // 播放使用物品的动画
        Debug.Log("鱼放在海鸭的餐桌");
        
        // 播放海鸭吃饭的TimeLine
        Invoke("TimeLineStart",1);
        dialogueController.ShowdialogueFinish();
    }
    private void TimeLineStart()
    {
        playableDirector.Play();
    }
    public override void EmptyClicked()
    {
        if(isDone)
            dialogueController.ShowdialogueFinish();
        else
            dialogueController.ShowdialogueEmpty();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Duck")
        {
            Debug.Log("鸭子开始吃饭");
            Animator anim = other.gameObject.GetComponent<Animator>();
            anim.SetBool("eat", true);
        }
    }
}
