using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFish : Singleton<CatchFish>
{
    // 引用预制件。在 Inspector 中，将预制件拖动到该字段中。
    public GameObject Fish1;
    public GameObject Fish2;
    public Vector3 Fish2StartOffest;
    TimerMgr timer;
    int TimerID;
    public Transform CameraController;
    public bool isRotate;
    public bool isLaunch;
    public GameObject DragBall1;
    public GameObject DragBall2;

    void Start()
    {
        // 初始化计时器
        timer = new TimerMgr();
        timer.Init();
        // 启动定时器
        TimerID = timer.Schedule(FishLaunch, 0, 4);
        isLaunch = true;
    }

    void Update()
    {
        if (timer != null && isLaunch)
            timer.Update();
        if(isRotate)
        CameraRotateRight();
        if(!isRotate)
        CameraRotateLeft();
    }
    
    private void FishLaunch()
    {
        // 实例化为位置 (0, 0, 0) 和零旋转。
        GameObject obj1 = Instantiate(Fish1, transform.position, Quaternion.LookRotation(new Vector3(0,-1,0)));
        GameObject obj2 = Instantiate(Fish2, transform.position + Fish2StartOffest, Quaternion.LookRotation(new Vector3(0,-1,0)));
        obj1.transform.SetParent(this.transform); 
        obj2.transform.SetParent(this.transform); 
    }
    public void Rotateleft()
    {
        isRotate = true;
    }
    public void Rotateright()
    {
        isRotate = false;
    }
    public void CameraRotateRight()
    {
        Vector3 dir = new Vector3(1,0,0);
        Quaternion targetQua = Quaternion.LookRotation(dir);
        CameraController.rotation = Quaternion.Slerp(CameraController.rotation,targetQua,2 * Time.deltaTime);
    }
    public void CameraRotateLeft()
    {
        Vector3 dir = new Vector3(0,0,1);
        Quaternion targetQua = Quaternion.LookRotation(dir);
        CameraController.rotation = Quaternion.Slerp(CameraController.rotation,targetQua,2 * Time.deltaTime);
    }

    // 初始化反弹球的位置，让UI调用
    public void InitializeDragBall()
    {
        DragBall1.transform.position = new Vector3(-2,12.21f,-8);
        DragBall2.transform.position = new Vector3(1,12.21f,-8);
    }
}
