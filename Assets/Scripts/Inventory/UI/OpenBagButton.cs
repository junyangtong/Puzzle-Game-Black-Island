using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBagButton : MonoBehaviour
{
    public Image itemImage;
    private void OnEnable()
    {
        EventHandler.SetOpenBagButtonEvent += OnSetOpenBagButtonEvent;
    }
    private void OnDisable() 
    {
        EventHandler.SetOpenBagButtonEvent -= OnSetOpenBagButtonEvent;
    }
    private void OnSetOpenBagButtonEvent(ItemDetails itemDetails)
    {
        itemImage.sprite = itemDetails.itemSprite;
        //itemImage.SetNativeSize();
    }
}
