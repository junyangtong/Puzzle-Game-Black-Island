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
            //靠近可拾取物品
            CanInteractive = true;
            Debug.Log("靠近可拾取物品" + "Layer层级：" + collision.gameObject.layer);
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        { 
            //离开可拾取物品
            CanInteractive = false;
            Debug.Log("离开可拾取物品" + "Layer层级：" + collision.gameObject.layer);
        }
    }
}
