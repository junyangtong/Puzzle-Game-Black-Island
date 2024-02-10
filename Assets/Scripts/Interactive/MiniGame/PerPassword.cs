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
    private bool isMoving = false;
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
        if(!isMoving)
        {
            isMoving = true;
            StartCoroutine(Change());
        }
    }
    /// <summary>
    /// 使用协程处理密码滚动动画 
    /// BUG:得到的浮点数有微小误差，在LockedBox中用 约等 的方法解决
    /// </summary>
    /// <returns></returns>
    IEnumerator Change()
    {
        float delta = 0.1f;   //delta为速度，每次加的数大小

        for(int i = 0;i<10;i++)
        {
            currentPassword += delta;
            //Debug.Log("+1");
            yield return new WaitForSeconds(0.01f);      //每 0.01s 加一次
        }
        isMoving = false;
        EventHandler.CallCheckGameStateEvent();
        StopCoroutine(Change());
    }
    void Update()
    {
        material.SetFloat("_Offest", currentPassword);
    }
}
