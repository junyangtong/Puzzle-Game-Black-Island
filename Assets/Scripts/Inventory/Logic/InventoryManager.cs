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
    [SerializeField] private List<ItemName> itemList = new List<ItemName>();
    public void AddItem(ItemName itemName)
    {
        if(!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            // UI对应显示
            itemDetails = itemData.GetItemDetails(itemName);
            CreateNewItem(itemDetails);
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
    public void SetGameObjectTag(GameObject gameObject, string tag)
    {
        if (!UnityEditorInternal.InternalEditorUtility.tags.Equals(tag)) // 如果tag列表中没有这个tag
        {
            UnityEditorInternal.InternalEditorUtility.AddTag(tag); // 在tag列表中添加这个tag
        }
        gameObject.tag = tag;
    }
}
