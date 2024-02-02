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
}
