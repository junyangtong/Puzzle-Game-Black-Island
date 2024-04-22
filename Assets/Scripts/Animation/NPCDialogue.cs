using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public Animation NPC;
    public AnimationClip IdleAnim;
    public AnimationClip TalkAnim;
    private void OnEnable() 
    {
        // NPC开始说话
        NPC.Play(TalkAnim.name);
    }
    private void OnDisable() 
    {
        // NPC停止说话
        NPC.Play(IdleAnim.name);

    }
}
