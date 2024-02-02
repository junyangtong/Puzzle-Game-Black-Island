using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : Interactive
{
    private CapsuleCollider coll;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider>();
    }
    
    protected override void OnClickedAction()
    {
        // 播放使用物品的动画
        Debug.Log("鸟蛋放在鸟窝里");
        // 关闭鸟窝的碰撞体
        coll.enabled = false;
    }
}
 