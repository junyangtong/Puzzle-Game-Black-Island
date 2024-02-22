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
    private ItemDetails noneItemDetails;

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
                InitializeSelectionState();
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
    public void InitializeSelectionState()
    {
        noneItemDetails = itemData.GetItemDetails(ItemName.None);
        isSelected = false;
        EventHandler.CallUpdateItemNameEvent(noneItemDetails.itemTooltip,isSelected);
        Debug.Log("初始化选择道具");
    }
}
