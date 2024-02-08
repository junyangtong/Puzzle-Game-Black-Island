using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private RaycastHit clickObject;
    private Ray ray;
    private ItemName currentItem;
    private bool holdItem;

    public CollisionEvents collisionEvents;

    private void OnEnable() 
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }
    void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {   
        holdItem = isSelected;
        if(isSelected)
        {
            currentItem = itemDetails.itemName;
            // TODO:替换角色手中的模型
        }
    }

    void Update()
    {
        //物品分类：
        //1.可拾取
        //2.可使用物品
        //3.可操作

        //如果角色靠近可交互物品
        
            //（0是左键、1是右键）
            if (Input.GetMouseButtonDown(0))
            {
                //向鼠标点击的位置发射射线
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //如果射线碰到物体
                if (Physics.Raycast(ray, out clickObject))
                {   
                    if(collisionEvents.CanInteractive == true)
                    {
                        switch(clickObject.transform.tag)
                        {
                            case "PickUpProp":
                                //执行拾取方法
                                var Pickeditem = clickObject.collider.gameObject.GetComponent<Item>();
                                Pickeditem?.ItemPicked(); 
                                collisionEvents.CanInteractive = false;
                                break;

                            case"InteractiveProp":
                                var interactive = clickObject.collider.gameObject.GetComponent<Interactive>();
                                if(holdItem)
                                    {
                                        interactive?.CheckItem(currentItem);
                                        //if(interactive.isDone)
                                            holdItem =false;//如果物品成功使用了 则取消选择状态
                                    }
                                else
                                    interactive?.EmptyClicked();
                                break;

                            case"CanBeUseProp":
                                var interactive1 = clickObject.collider.gameObject.GetComponent<Interactive>();
                                interactive1?.ClickItem();
                                break;
                        }
                    }
                        //不需要碰撞检测就可以触发点击 一般用于小游戏中的道具
                        switch(clickObject.transform.tag)
                            {
                                case "FreelyUseProp":
                                    var interactive2 = clickObject.collider.gameObject.GetComponent<Interactive>();
                                    interactive2?.ClickItem();
                                    break;
                            }
                        
                }
            }
    }
    
     /// <summary>
    /// 与此类无关 初始化背包
    /// </summary>
    private void Awake() {
        //开始前创建空物品栏
        InventoryManager.Instance.AddItem(ItemName.None);
        EventHandler.CallUpdateItemNameEvent(ItemName.None,false);
    }
}
