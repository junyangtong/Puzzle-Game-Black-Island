using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(RelativePositonForAnimation))]
public class AiMove : MonoBehaviour
{

    public float speed = 3;
    public float rotateLerp = 0.1f; // 旋转插值比例
    public bool isMove = false;
    public AnimationClip FishShoked;
    private Animation anim;
    private CharacterController car;
    private Vector3 targetDirection,currentDirection;
    public Vector2 Min, Max;
    private Vector2 TargetPoint;
    private Vector2 MoveTarget;
    private Vector3 RepulsedTarget;
    private bool isRepulsed = false;
    public bool isShoked = false;
    private bool closeYuPiao = false;
    private Vector3 YupiaoPos;
    //计时器
    TimerMgr timer;
    int TimerID;
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();   
         // 初始化计时器
        timer = new TimerMgr();
        timer.Init();
        // 启动计时器
        TimerID = timer.Schedule(MoveOver, 1, 3);
        car = GetComponent<CharacterController>();
    }
    private void MoveOver()
    {
        isMove = false;
    }
    void Update()
    {   
        if(!isShoked)
        {
            Vector3 move = Vector3.zero;
            // 获取随机目标点
            if(!isMove)
            {
                TargetPoint.x = Random.Range(Min.x, Max.x);
                TargetPoint.y = Random.Range(Min.y, Max.y);
                isMove = true;
                MoveTarget = (TargetPoint).normalized;
            }
            if(isRepulsed)
            {
                MoveTarget = new Vector2((RepulsedTarget).normalized.x,(RepulsedTarget).normalized.z);
                isRepulsed = false;
            }
            if(closeYuPiao)
            {
                Vector3 dir = YupiaoPos - transform.position;
                MoveTarget = new Vector2((dir).normalized.x,(dir).normalized.z);
            }
            if (isMove)
            {
                // 计时器控制移动时间
                timer.Update();
            }
            
            
            // 角色移动
            move.x = MoveTarget.x * Time.deltaTime;
            move.z = MoveTarget.y * Time.deltaTime;

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
        }

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
            RepulsedTarget = hit.normal;
            isRepulsed = true;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        // 如果主角靠近魚 魚游走
        if(other.gameObject.tag == "Player")
        {
            anim.Play(FishShoked.name);
        }
        // 如果靠近鱼漂 上钩
        if(other.gameObject.tag == "Yupiao")
        {
            // 只有 鱼漂没有鱼上钩时触发
            if(!Yupiao.Instance.hasFish)
            {
                Debug.Log("靠近鱼漂");
                // 获取鱼漂的局部坐标
                YupiaoPos = other.gameObject.transform.position;
                closeYuPiao = true;
                Yupiao.Instance.hasFish = true;
                Invoke("beCatch",5);
            }
            
        }
    }
    private void OnTriggerStay(Collider other) 
    {
        // 如果主角靠近魚 魚游走
        if(other.gameObject.tag == "Player")
        {
            anim.Play(FishShoked.name);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        // 只对即将上钩的鱼生效
        if(closeYuPiao)
        {
            // 如果中途停止钓鱼 鱼继续自由活动
            closeYuPiao = false;
            Yupiao.Instance.hasFish = false;
        }
        
    }
    private void beCatch()
    {
        if(Yupiao.Instance.hasFish)
        {
            Yupiao.Instance.Catching = true;
            this.gameObject.SetActive(false);
        }
        else
        {
            Yupiao.Instance.Catching = false;
        }
    }
}