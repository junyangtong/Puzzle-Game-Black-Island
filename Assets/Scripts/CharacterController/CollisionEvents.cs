using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvents : MonoBehaviour
{
    public bool CanInteractive = false;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        { 
            //进入可交互物品
            CanInteractive = true;
            Debug.Log("进入可交互物品" + collision.gameObject.name);
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        { 
            //在可交互物品范围内
            CanInteractive = true;
            //Debug.Log("在可交互物品范围内" + "Layer层级：" + collision.gameObject.layer);
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        { 
            // 离开可交互物品
            CanInteractive = false;
            Debug.Log("离开可交互物品" + collision.gameObject.name);
            
            // 离开可交互物品时取消对话框并 恢复为游戏进行状态
            EventHandler.CallShowDialogueEvent(string.Empty);
            EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        }
    }
}
