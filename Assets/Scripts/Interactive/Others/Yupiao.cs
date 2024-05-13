using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(RelativePositonForAnimation))]
public class Yupiao : Singleton<Yupiao>
{
    public bool hasFish = false;
    public bool Catching = false;
    private Animation anim;
    public AnimationClip Pickyupiao;
    private bool AnimStart = false;
    
    private void OnEnable() 
    {
        anim = gameObject.GetComponent<Animation>();   
        anim.playAutomatically = false;
        if(Pickyupiao == null)
            Debug.LogError("请指定收起鱼漂的动画");
    }
    private void Update()
    {
        if(AnimStart)
        {
            if(!anim.isPlaying)
            {
                // 动画播放结束隐藏物体
                AnimStart = false;
                Destroy(this.gameObject); 
                Debug.Log("动画播放结束");
            }
        }
        if(Catching) 
        {
            Debug.Log("鱼上钩啦！");
        }
    }
    public void PickYupiao() 
    {
        anim.Play(Pickyupiao.name);
        AnimStart = true;
    }
}
