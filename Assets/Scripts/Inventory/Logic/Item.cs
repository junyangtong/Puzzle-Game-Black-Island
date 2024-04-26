using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(RelativePositonForAnimation))]

public class Item : MonoBehaviour
{
    public AnimationClip PickItemAnim;
    private Animation anim;
    public ItemName itemName;
    private bool AnimStart = false;
    public bool CanInteractive = false;
    //private string pickUpProp = "PickUpProp";
    private void Update()
    {
        if(AnimStart)
        {
            if(!anim.isPlaying)
            {
                // 动画播放结束隐藏物体
                this.gameObject.SetActive(false);
                Debug.Log("动画播放结束");
            }
        }
    }
    private void Start() 
    {
        anim = gameObject.GetComponent<Animation>();   
        anim.playAutomatically = false;
        if(PickItemAnim != null)
            anim.AddClip(PickItemAnim, PickItemAnim.name);
        else
        Debug.LogError("请指定拾取物品动画");
    }
    private void Awake() 
    {
        
    }
    public void ItemPicked()
    {
        // 添加到背包后播放对应动画
        InventoryManager.Instance.AddItem(itemName);
        anim.Play(PickItemAnim.name);
        AnimStart = true;
        // 角色播放交互动画
        EventHandler.CallItemCheckAnim(true);
        
    }
}
