using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvents : MonoBehaviour
{
    public bool isCloseObj = false;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        { 
            // 如果是可拾取道具
            if(collision.gameObject.tag == "PickUpProp")
            {
                collision.gameObject.GetComponent<Item>().CanInteractive = true;
            }
            // 如果是可交互道具
            else
            {
                collision.gameObject.GetComponent<Interactive>().CanInteractive = true;
            }
            // 进入可交互物品
            isCloseObj = true;
            Debug.Log("进入可交互物品" + collision.gameObject.name);
        }
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        { 
            // 如果是可拾取道具
            if(collision.gameObject.tag == "PickUpProp")
            {
                collision.gameObject.GetComponent<Item>().CanInteractive = true;
            }
            // 如果是可交互道具
            else
            {
                collision.gameObject.GetComponent<Interactive>().CanInteractive = true;
            }
            //在可交互物品范围内
            isCloseObj = true;
            //Debug.Log("在可交互物品范围内" + "Layer层级：" + collision.gameObject.layer);
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        { 
            // 如果是可拾取道具
            if(collision.gameObject.tag == "PickUpProp")
            {
                collision.gameObject.GetComponent<Item>().CanInteractive = false;
            }
            // 如果是可交互道具
            else
            {
                collision.gameObject.GetComponent<Interactive>().CanInteractive = false;
            }
            // 离开可交互物品
            isCloseObj = false;
            Debug.Log("离开可交互物品" + collision.gameObject.name);
            
            // 离开可交互物品时取消对话框并 恢复为游戏进行状态
            EventHandler.CallShowDialogueEvent(string.Empty);
            EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        }
    }
}
