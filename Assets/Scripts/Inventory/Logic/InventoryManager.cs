using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{   
    public ItemDataList_SO itemData;
    public ItemDetails itemDetails;
    public GameObject slotGrid;
    public GameObject slotPrefab;   // prefab
    public bool isSelected;
    [SerializeField] private List<ItemName> itemList = new List<ItemName>();
    private void OnEnable()
    {
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }
    private void OnDisable() 
    {
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
    }
    //使用物品时移除背包中的对应物品
    private void OnItemUsedEvent(ItemName itemName)
    {
        int children = slotGrid.transform.childCount;
        for (int i = 0; i < children; i++)
        {   
            SlotUI slotUI = slotGrid.transform.GetChild(i).GetComponent<SlotUI>();
            if(slotUI.currentItem.itemName == itemName)
            {
                EventHandler.CallUpdateItemNameEvent(ItemName.None,false);
                Destroy(slotUI.gameObject);
                Debug.Log("移除背包中的"+slotUI.currentItem.itemName);
                itemList.Remove(itemName);
            }
        }
        
    }
    public void AddItem(ItemName itemName)
    {
        if(!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            // UI对应显示
            itemDetails = itemData.GetItemDetails(itemName);
            CreateNewItem(itemDetails);
        }
        else
        {
            Debug.Log("请勿重复添加，已销毁");
        }
    }
    public void CreateNewItem(ItemDetails itemDetails)
    {
        GameObject newItem = Instantiate(slotPrefab,slotGrid.transform.position,Quaternion.identity);
        newItem.transform.SetParent(slotGrid.transform);
        // 设置实例化物体的参数
        SlotUI slotUI = newItem.GetComponent<SlotUI>();
        slotUI.SetItem(itemDetails);
        Debug.Log("拾取"+itemDetails.itemName);
    }

    //运行时设置物体Tag
    /*public void SetGameObjectTag(GameObject gameObject, string tag)
    {
        if (!UnityEditorInternal.InternalEditorUtility.tags.Equals(tag)) // 如果tag列表中没有这个tag
        {
            UnityEditorInternal.InternalEditorUtility.AddTag(tag); // 在tag列表中添加这个tag
        }
        gameObject.tag = tag;
    }*/
}
