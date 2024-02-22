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
    public void UpdateItemName(string itemTooltip,bool isSelected)
    {
        itemNameText.text = itemTooltip;
    }
}
