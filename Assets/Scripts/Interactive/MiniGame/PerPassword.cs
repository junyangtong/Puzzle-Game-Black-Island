using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerPassword : Interactive
{
    private Material material;
    public float amount = 7.0f;
    private float nowAmount;
    public float currentPassword = 1.0f;
    public float loopCount;
    private bool isMatch;
    private void Awake() {
        material = gameObject.GetComponent<MeshRenderer>().material;
        // 初始化密码
        material.SetFloat("_Offest", currentPassword);

        nowAmount = amount;
        
        loopCount = 0.0f;
    }
    
    protected override void OnClickedAction()
    {
        // 下一个密码
        NextPassword();
        UpdateLoopCount();
        EventHandler.CallCheckGameStateEvent();
    }
    private void UpdateLoopCount()
    {
        if(currentPassword > nowAmount)
        {
            loopCount += 1.0f;
            nowAmount += amount;
        }
    }
    private void NextPassword()
    {
        currentPassword += 1.0f;
    }
    void Update()
    {
        material.SetFloat("_Offest", currentPassword);
    }
}
