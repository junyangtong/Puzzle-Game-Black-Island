using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour,IPointerClickHandler
{
    public Image itemImage;
    public ItemDetails currentItem;
    private bool isSelected;
    private bool isSelectedtemp;
    public void SetItem(ItemDetails itemDetails)
    {
        currentItem = itemDetails;
        itemImage.sprite = itemDetails.itemSprite;
        itemImage.SetNativeSize();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 设置isSelected
        isSelectedtemp = InventoryManager.Instance.isSelected;
        
        if(currentItem.itemName == ItemName.None)
            isSelectedtemp = false;
        else
            isSelectedtemp = true;

        InventoryManager.Instance.isSelected = isSelectedtemp;
        isSelected = InventoryManager.Instance.isSelected;
        
        // 呼叫方法 修改提示词
        EventHandler.CallUpdateItemNameEvent(currentItem.itemTooltip,isSelected);
        
        // 呼叫方法 判断是否选择物品
        EventHandler.CallItemSelectedEvent(currentItem,isSelected);

        // 呼叫方法 设置打开背包按钮UI
        EventHandler.CallSetOpenBagButtonEvent(currentItem);
    }
    // 在InventoryManager调用
    public void HighLight(bool isHighLight)
    {
        // 高亮显示
        if(isHighLight)
            itemImage.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        else
            itemImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    
}
