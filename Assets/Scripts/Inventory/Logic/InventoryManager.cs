using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField] private List<ItemName> itemList = new List<ItemName>();
    public void AddItem(ItemName itemName)
    {
        if(!itemList.Contains(itemName))
        {
            itemList.Add(itemName);
            //TODO:UI对应显示
        }
    }
    public void SetGameObjectTag(GameObject gameObject, string tag)
    {
        if (!UnityEditorInternal.InternalEditorUtility.tags.Equals(tag)) //如果tag列表中没有这个tag
        {
            UnityEditorInternal.InternalEditorUtility.AddTag(tag); //在tag列表中添加这个tag
        }

        gameObject.tag = tag;
    }
}
