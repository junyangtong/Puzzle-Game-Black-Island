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
    public BagUI bagUI;   
    public bool isSelected;
    public bool holdItem;
    public ItemName currentItem;
    private ItemDetails noneItemDetails;

    [SerializeField] private List<ItemName> itemList = new List<ItemName>();
    /// <summary>
    /// 初始化背包
    /// </summary>
    private void Start() 
    {
        //开始前创建空物品栏
        AddItem(ItemName.None);
        InitializeSelectionState();
    }
    private void OnEnable()
    {
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }
    private void OnDisable() 
    {
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }
    private void OnItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {   
         // 高亮显示
        HighLightItem(itemDetails.itemName);
        holdItem = isSelected;
        if(isSelected)
        {
            currentItem = itemDetails.itemName;
            Debug.Log("当前选择"+itemDetails.itemName);
            // TODO:替换角色手中的模型
        }
        else
        {
            Debug.Log("未选择");
        } 
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
            bagUI.BagOpen();
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
    private void HighLightItem(ItemName itemName)
    {
        int children = slotGrid.transform.childCount;
        for (int i = 0; i < children; i++)
        {   
            SlotUI slotUI = slotGrid.transform.GetChild(i).GetComponent<SlotUI>();
            if(slotUI.currentItem.itemName == itemName)
            {
               slotUI.HighLight(true);
            }
            else
               slotUI.HighLight(false);
        }
    }
    public void InitializeSelectionState()
    {
        noneItemDetails = itemData.GetItemDetails(ItemName.None);
        isSelected = false; 
        EventHandler.CallUpdateItemNameEvent(noneItemDetails.itemTooltip,isSelected);
        HighLightItem(ItemName.None);
        EventHandler.CallSetOpenBagButtonEvent(noneItemDetails);// 初始化打开背包按钮UI
        currentItem = noneItemDetails.itemName;
        holdItem = false;
        Debug.Log("初始化背包状态");
    }
}
