using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFish : MonoBehaviour
{
    // 引用预制件。在 Inspector 中，将预制件拖动到该字段中。
    public GameObject obstacle;
    TimerMgr timer;
    int TimerID;

    void Start()
    {
        //初始化
        timer = new TimerMgr();
        timer.Init();
        //启动定时器
        TimerID = timer.Schedule(FishLaunch, 0, 2);
    }

    void Update()
    {
        timer.Update();
    }
    
    private void FishLaunch()
    {
        // 实例化为位置 (0, 0, 0) 和零旋转。
        Instantiate(obstacle, transform.position, Quaternion.identity);
    }
}
