using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Image itemImage;
    private ItemDetails currentItem;
    public void SetItem(ItemDetails itemDetails)
    {
        currentItem = itemDetails;
        itemImage.sprite = itemDetails.itemSprite;
        itemImage.SetNativeSize();
    }
    
}
