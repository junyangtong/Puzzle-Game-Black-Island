// 交互逻辑基类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemName requireItem;
    public bool isDone;
    public bool CanInteractive = false;

    /// <summary>
    /// 需要使用背包中的物品交互
    /// </summary>
    /// <param name="itemName"></param>
    public void CheckItem(ItemName itemName)
    {
        if(itemName == requireItem && !isDone)
        {
            // 角色播放使用物品动画
            EventHandler.CallItemCheckAnim(true);
            // TODO:支持可以多次使用的物品 当前只单独支持钓鱼竿
            if(itemName != ItemName.FishingRods)
            {
                isDone = true;
            }
            // 使用这个物品，移除物品
            OnClickedAction();
            EventHandler.CallItemUsedEvent(itemName);
        }
        if(itemName != requireItem) // && !isDone
        {
            // 角色播放使用物品错误动画
            EventHandler.CallItemCheckAnim(false);
            InventoryManager.Instance.InitializeSelectionState();
            Debug.Log("物品错误无法使用");
        }
    } 
    /// <summary>
    /// 不需要使用物品就可以交互
    /// </summary>
    public void OnClickedActionNoRequire()
    {
        // 角色播放使用物品动画
        EventHandler.CallItemCheckAnim(true);
        OnClickedAction();
    }
    /// <summary>
    /// 默认是正确物品的情况执行
    /// <summary>
    protected virtual void OnClickedAction()
    {

    }
    public virtual void EmptyClicked()
    {
        Debug.Log("空点");
    }
}
