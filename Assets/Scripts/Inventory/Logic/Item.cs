using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemName itemName;
    public GameObject PickBubble;
    private string pickUpProp = "PickUpProp";
    private void Awake() {
        //设置可拾取物品Tag
        InventoryManager.Instance.SetGameObjectTag(this.gameObject,pickUpProp);

        PickBubble.SetActive(false);
    }
    public void ItemPicked()
    {
        //添加到背包后隐藏物体
        InventoryManager.Instance.AddItem(itemName);
        this.gameObject.SetActive(false);
    }
    public void PickBubbleAppear()
    {
        PickBubble.SetActive(true);
    }
    public void PickBubbleDisAppear()
    {
        PickBubble.SetActive(false);
    }
}
