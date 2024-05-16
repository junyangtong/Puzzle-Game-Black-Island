using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBagButton : MonoBehaviour
{
    public Image itemImage;
    public Sprite NoneImage;
    public GameObject BackOpenBagButton;
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
        if(itemDetails.itemName == ItemName.None)
        {
            itemImage.sprite = NoneImage;
            BackOpenBagButton.SetActive(false);
        }
        else
        {
            BackOpenBagButton.SetActive(true);
            itemImage.sprite = itemDetails.itemSprite;
        }
        
        //itemImage.SetNativeSize();
    }
}
