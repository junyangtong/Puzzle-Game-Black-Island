using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1WoodDoor : Interactive
{
    protected override void OnClickedAction()
    {
        // 点击时打开木门
        this.gameObject.SetActive(false);
    }
}
