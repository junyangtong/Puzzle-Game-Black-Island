using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvents : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "PickUpProp"){};
        { 
            //气泡出现
            var Pickeditem = collision.gameObject.GetComponent<Item>();
            Pickeditem?.PickBubbleAppear(); 
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "PickUpProp"){};
        { 
            //气泡消失
            var Pickeditem = collision.gameObject.GetComponent<Item>();
            Pickeditem?.PickBubbleDisAppear(); 
        }
    }
}
