using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AiMove : MonoBehaviour
{

    public float speed = 3;
    public float rotateLerp = 0.1f; // 旋转插值比例
    public bool isMove = false;
    private CharacterController car;
    private Vector3 targetDirection,currentDirection;
    public Vector2 Min, Max;
    private Vector2 TargetPoint;
    private Vector2 MoveTarget;
    private Vector3 RepulsedTarget;
    private bool isRepulsed = false;
    //计时器
    TimerMgr timer;
    int TimerID;
    void Start()
    {
         // 初始化计时器
        timer = new TimerMgr();
        timer.Init();
        // 启动计时器
        TimerID = timer.Schedule(MoveOver, 1, 2);
        car = GetComponent<CharacterController>();
    }
    private void MoveOver()
    {
        isMove = false;
        isRepulsed = false;
    }
    void Update()
    {   
        Vector3 move = Vector3.zero;
        // 获取随机目标点
        if(!isMove && !isRepulsed)
        {
            TargetPoint.x = Random.Range(Min.x, Max.x);
            TargetPoint.y = Random.Range(Min.y, Max.y);
            isMove = true;
            MoveTarget = (TargetPoint).normalized;
        }
        if(isRepulsed)
        {
            MoveTarget = (RepulsedTarget).normalized;
            isMove = true;
        }
        // 角色移动
        move.x = MoveTarget.x * Time.deltaTime;
        move.z = MoveTarget.y * Time.deltaTime;

        // 计时器控制移动时间
        timer.Update();
        //move.y -= 3f * Time.deltaTime;  //模拟重力
        if (car != null)
        {
            car.Move(move*speed);
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
        if (hit.gameObject.layer == 11)
        { 
            Debug.Log("被击退");
            RepulsedTarget = hit.normal;
            isRepulsed = true;
        }
    }
    
}