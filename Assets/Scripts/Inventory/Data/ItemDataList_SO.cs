using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_SO", menuName = "Inventory/ItemDataList_SO")]
public class ItemDataList_SO : ScriptableObject {
    public List<ItemDetails> itemDatailsList;
    public ItemDetails GetItemDetails(ItemName itemName)
    {
        Debug.Log("GetItemDetails");
        return itemDatailsList.Find(i => i.itemName == itemName);    
    }

}
//创建物品数据结构（需要序列化）
[System.Serializable]
public class ItemDetails
{
    public ItemName itemName;
    public Sprite itemSprite;
}