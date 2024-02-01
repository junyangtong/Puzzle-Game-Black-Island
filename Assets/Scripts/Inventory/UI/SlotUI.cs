using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour,IPointerClickHandler
{
    public Image itemImage;
    private ItemDetails currentItem;
    private bool isSelected;
    public void SetItem(ItemDetails itemDetails)
    {
        currentItem = itemDetails;
        itemImage.sprite = itemDetails.itemSprite;
        itemImage.SetNativeSize();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        //呼叫方法
        EventHandler.CallItemSelectedEvent(currentItem,isSelected);

    }
    
}
