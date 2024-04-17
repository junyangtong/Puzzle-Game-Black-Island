using System;
using UnityEngine;

public static class EventHandler
{
    public static event Action<ItemDetails,bool>ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails,isSelected);
    }

    public static event Action<ItemName> ItemUsedEvent;
    public static void CallItemUsedEvent(ItemName itemName)
    {
        ItemUsedEvent?.Invoke(itemName);
    }

    //对话
    public static event Action<string> ShowDialogueEvent;
    public static void CallShowDialogueEvent(string dialogue)
    {
        ShowDialogueEvent?.Invoke(dialogue);
    }

    //游戏状态
    public static event Action<GameState> GameStateChangeEvent;
    public static void CallGameStateChangeEvent(GameState gameState)
    {
        GameStateChangeEvent?.Invoke(gameState);
    }

    //提示标签
    public static event Action<string,bool> UpdateItemNameEvent;
    public static void CallUpdateItemNameEvent(string itemTooltip,bool isSelected)
    {
        UpdateItemNameEvent?.Invoke(itemTooltip,isSelected);
    }

    //检查密码是否匹配
    public static event Action CheckGameStateEvent;
    public static void CallCheckGameStateEvent()
    {
        CheckGameStateEvent?.Invoke();
    }
    //角色开始移动时调整Target的位置
    public static event Action<bool> OffsetCharacterTarget;
    public static void CallOffsetCharacterTarget(bool isMove)
    {
        OffsetCharacterTarget?.Invoke(isMove);
    }
    //设置背包按钮
    public static event Action<ItemDetails>SetOpenBagButtonEvent;
    public static void CallSetOpenBagButtonEvent(ItemDetails itemDetails)
    {
        SetOpenBagButtonEvent?.Invoke(itemDetails);
    }
    //角色瞬移
    public static event Action<Vector3> TeleportEvent;
    public static void CallTeleportEvent(Vector3 targetPos)
    {
        TeleportEvent?.Invoke(targetPos);
    }
}
