using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1WoodDoor : Interactive
{
    public int ClickNum = 0;
    private Animation anim;
    private bool AnimStart = false;
    public AnimationClip First;
   // public AnimationClip Second;
    //public AnimationClip Third;

    private void Update()
    {
        if(AnimStart)
        {
            if(!anim.isPlaying)
            {
                Debug.Log("动画播放结束");
                AnimStart = false;
            }
        }
    }
    private void Start() 
    {
        anim = gameObject.GetComponent<Animation>();  
        anim.AddClip(First,First.name); 
        //anim.AddClip(Second,Second.name); 
        //anim.AddClip(Third,Third.name); 
    }
    protected override void OnClickedAction()
    {
        
        // 只有不播放动画的时候才会累加次数
        if(!AnimStart)
        {
            ClickNum++;
        }
        // 点击第一次时打开木门
        if(ClickNum == 1 && !AnimStart)
        {
            anim.Play(First.name);
            AnimStart=true;
            Debug.Log("第一次点击");
        }
        if(ClickNum == 2 && !AnimStart)
        {
            anim.Play(First.name);
            AnimStart=true;
            Debug.Log("第二次点击");
        }
        if(ClickNum >= 3 && !AnimStart)
        {
            // 门被打开
            this.gameObject.SetActive(false);
            Debug.Log("第二次点击门被打开");
        }
        
    }
}
