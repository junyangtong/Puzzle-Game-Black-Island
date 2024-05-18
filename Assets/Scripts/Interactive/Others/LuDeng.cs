using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuDeng : Interactive
{
    public Bird bird;
    public MeshRenderer meshRenderer;
    private bool birdOnLight;
    public override void EmptyClicked()
    {
        if(isDone)
        {
            // 关灯
            //更改物体Material的颜色（RGBA来调整） R/G/B/A都是一个float类型的数
            meshRenderer.material.SetColor("_EmissCol", new Color(9.18f,4.17f,0,0));
            isDone = false;
            EventHandler.CallItemCheckAnim(true);
        }
        else
        {
            // 开灯
            meshRenderer.material.SetColor("_EmissCol", new Color(0,0,0,0));
            isDone = true;
            EventHandler.CallItemCheckAnim(true);
        }
        if(birdOnLight)
        {
            // 从路灯到钩子
            bird.FromLightToGouzi();
        }
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "CanBeUseProp")
        {
            birdOnLight = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "CanBeUseProp")
        {
            birdOnLight = false;
        }
    }

}
