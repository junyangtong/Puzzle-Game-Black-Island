using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private RaycastHit clickObject;
    private Ray ray;
    private ItemName currentItem;
    private bool holdItem;
    private bool canClick;

    public CollisionEvents collisionEvents;

    private void OnEnable() 
    {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
    }
    private void OnDisable() 
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
    }
    private void OnGameStateChangeEvent(GameState gameState)
    {
        canClick = gameState == GameState.GamePlay;
    }

    void Update()
    {
        //物品分类：
        //1.可拾取
        //2.可使用物品
        //3.可操作

        //如果角色靠近可交互物品
        
            //（0是左键、1是右键）
            if (Input.GetMouseButtonDown(0) && canClick)
            {
                //向鼠标点击的位置发射射线
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //如果射线碰到物体
                if (Physics.Raycast(ray, out clickObject))
                {   
                    //Debug.Log(clickObject.collider.gameObject.name);
                    if(collisionEvents.isCloseObj == true)
                    {
                        switch(clickObject.transform.tag)
                        {
                            case "PickUpProp":
                                //执行拾取方法
                                var Pickeditem = clickObject.collider.gameObject.GetComponent<Item>();
                                if(Pickeditem.CanInteractive)
                                {
                                    Pickeditem?.ItemPicked(); 
                                }
                                Pickeditem.CanInteractive = false;
                                break;

                            case "InteractiveProp":
                                var interactive = clickObject.collider.gameObject.GetComponent<Interactive>();
                                if(interactive.CanInteractive)
                                {
                                    holdItem = InventoryManager.Instance.holdItem;
                                    if(holdItem)
                                        {
                                            currentItem = InventoryManager.Instance.currentItem;
                                            interactive?.CheckItem(currentItem);
                                            //if(interactive.isDone)
                                                holdItem =false;//如果物品成功使用了 则取消选择状态
                                        }
                                    else
                                        interactive?.EmptyClicked();
                                }
                                    break;

                            case "CanBeUseProp":
                                var interactive1 = clickObject.collider.gameObject.GetComponent<Interactive>();
                                if(interactive1.CanInteractive)
                                {
                                    interactive1?.OnClickedActionNoRequire();
                                }
                                break;
                        }
                    }
                        //不需要碰撞检测就可以触发点击 一般用于小游戏中的道具
                        switch(clickObject.transform.tag)
                            {
                                case "FreelyUseProp":
                                    var interactive2 = clickObject.collider.gameObject.GetComponent<Interactive>();
                                    interactive2?.OnClickedActionNoRequire();
                                    break;
                            }
                        
                }
            }
    }
}
