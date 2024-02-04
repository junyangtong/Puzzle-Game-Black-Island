using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;
    private void OnEnable() 
    {
        EventHandler.UpdateItemNameEvent += UpdateItemName;
    }
    private void OnDisable() 
    {
        EventHandler.UpdateItemNameEvent -= UpdateItemName;
    }
    public void UpdateItemName(ItemName itemName,bool isSelected)
    {
        if(isSelected)
        {
            Debug.Log("当前选择"+itemName);
        }
        else
        {
            Debug.Log("未选择");
        }
        itemNameText.text = itemName switch
        {
            ItemName.Egg => "一个普通的蛋",
            ItemName.Seed => "一颗种子",
            ItemName.None => "未选择...",
            _ => ""
        };
        
    }
}
