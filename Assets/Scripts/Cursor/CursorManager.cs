using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private RaycastHit clickBubble;
    private Ray ray;
    
    void Update()
    {
        //TODO：点击气泡
        //1.可拾取
        //2.可使用物品
        //3.可操作

        //（0是左键、1是右键）
        if (Input.GetMouseButtonDown(1))
        {
            //向鼠标点击的位置发射射线
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //如果射线碰到物体
            if (Physics.Raycast(ray, out clickBubble))
            {
                switch(clickBubble.transform.tag)
                {
                    case "PickBubble":
                        //气泡父物体执行拾取方法
                        var Pickeditem = clickBubble.collider.gameObject.transform.parent.GetComponent<Item>();
                        Pickeditem?.ItemPicked(); 
                        break;
                }
            }
        }
    }
}
