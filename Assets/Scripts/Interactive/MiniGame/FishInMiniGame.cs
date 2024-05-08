using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FishInMiniGame : MonoBehaviour
{
    public float speed = 3;
    public float rotateLerp = 0.1f; // 旋转插值比例
    private CharacterController car;
    private Vector3 targetDirection,currentDirection;
    private Vector3 MoveTarget;
    private Vector3 RepulsedTarget;
    private bool isRepulsed = false;
    private bool isRepulsing = false;
    public Vector3 StartDir = new Vector3(0,-1,0);
    //计时器
    TimerMgr timer;
    int TimerID;
    void Start()
    {
         // 初始化计时器
        timer = new TimerMgr();
        timer.Init();
        // 启动计时器
        TimerID = timer.Schedule(MoveOver, 1, 3);
        car = GetComponent<CharacterController>();
        // 初始化移动方向
        MoveTarget = (StartDir).normalized;
    }
    private void MoveOver()
    {
        isRepulsing = false;
        //Debug.Log("保护结束");
    }
    void Update()
    {   
        Vector3 move = Vector3.zero;
        if(isRepulsed)
        {
            MoveTarget = RepulsedTarget.normalized;
            isRepulsed = false;
        }
        
        // 角色移动
        move.x = MoveTarget.x * Time.deltaTime;
        move.y = MoveTarget.y * Time.deltaTime;
        move.z = MoveTarget.z * Time.deltaTime;

        //move.y -= 3f * Time.deltaTime;  //模拟重力
        if (car != null)
        {
            car.Move(move*speed);
        }
        if(isRepulsing)
        {
            // 碰撞保护时间防止连续碰撞
            timer.Update();
            //Debug.Log("不可碰撞");
        }

         // 角色旋转
        currentDirection = transform.forward.normalized;
            
        if(move.x!=0 || move.z !=0)
        {
            targetDirection = new Vector3(move.x,0f,move.z);
        }
        else
        {
            targetDirection = currentDirection;
        }

        //float rotationAngle = Mathf.Acos(Vector3.Dot(targetDirection, currentDirection)) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotateLerp);

        //transform.LookAt(transform.position + targetDirection); 
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(Max.x, 0.1f, Max.y));
    }*/
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 检测是否处于可交互物品的保护范围内 如果是则被击退
        if(!isRepulsing)
        {
            if (hit.gameObject.layer == 11)
            { 
                RepulsedTarget = hit.normal;
                isRepulsed = true;
                isRepulsing = true;
            }
        }
        
    }

    // TODO: 游戏关闭时应该销毁所有鱼

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 10)
        { 
            //进入管道
            Debug.Log("进入管道，游戏结束");
            CatchFish.Instance.isLaunch = false;
            
        }
    }
}
