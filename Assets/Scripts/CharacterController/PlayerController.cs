using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public float rotateLerp = 0.1f; // 旋转插值比例
    public GameObject StepParticall;
    public GameObject StepParticalr;
    [Header("当被击退时")]
    public float Repulsedspeed;

    private CharacterController car;
    private Animator anim;
    private Transform player;
    private Vector3 targetDirection,currentDirection;
    private bool canMove = false;
    private bool isMove = false;
    private bool isRepulsed = false;
    private Vector3 RepulsedTarget;

    //计时器
    TimerMgr timer;
    int TimerID;
    
    private void OnEnable() 
    {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
        EventHandler.TeleportEvent += OnTeleportEvent;
    }
    private void OnDisable() 
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        EventHandler.TeleportEvent -= OnTeleportEvent;
    }
    private void OnGameStateChangeEvent(GameState gameState)
    {
        canMove = gameState == GameState.MiniGame;
    }
    private void OnTeleportEvent(Vector3 targetPos)
    {   
        this.gameObject.transform.position = targetPos;
        Debug.Log("瞬移");
    }
    void Start()
    {
        // 初始化计时器
        timer = new TimerMgr();
        timer.Init();
        // 启动计时器
        TimerID = timer.Schedule(RepulsedOver, 1, 1);

        car = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        player = this.transform;
    }

    void Update()
    {   
        // 角色移动
        Vector3 move = Vector3.zero;
        if(!canMove)
        {   
            // 如果角色在被击退状态
            if(isRepulsed)
            {   
                // 计时器控制击退时间
                timer.Update();
                Vector3 RepulsedVector = (transform.position - RepulsedTarget).normalized;
                move.x = RepulsedVector.x * Time.deltaTime;
                move.z = RepulsedVector.z * Time.deltaTime;
            }
            else
            {
                move.x = Input.GetAxis("Horizontal") * Time.deltaTime;
                move.z = Input.GetAxis("Vertical") * Time.deltaTime;
            }
            move.y -= 3f * Time.deltaTime;  //模拟重力
        }
        if (car != null)
        {
            car.Move(move*speed);
        }

        // 角色旋转
        currentDirection = transform.forward.normalized;
            
        if(move.x!=0 || move.z !=0)
        {
            targetDirection = new Vector3(move.x,0f,move.z);
            anim.SetBool("isMoving",true);
            isMove = true;
            //StepParticall.SetActive(true);
            //StepParticalr.SetActive(true);
            EventHandler.CallOffsetCharacterTarget(isMove);
        }
        else
        {
            anim.SetBool("isMoving",false);
            targetDirection = currentDirection;
            isMove = false;
            //StepParticall.SetActive(false);
            //StepParticalr.SetActive(false);
            EventHandler.CallOffsetCharacterTarget(isMove);
        }
        if(!canMove)
        {
        //float rotationAngle = Mathf.Acos(Vector3.Dot(targetDirection, currentDirection)) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotateLerp);

        //transform.LookAt(transform.position + targetDirection); 
        }
    }
    private void RepulsedOver()
    {   
        // 结束击退
        isRepulsed = false;
        // 结束计时
        timer.Unschedule(TimerID);
        TimerID = timer.Schedule(RepulsedOver, 1, 1);
        Debug.Log("击退结束");
    }
    
    void OnTriggerEnter(Collider collision)
    {
        // 检测是否处于可交互物品的保护范围内 如果是则被击退
        if (collision.gameObject.layer == 11)
        { 
            Debug.Log("被击退");
            RepulsedTarget = collision.gameObject.transform.position;
            isRepulsed = true;
        }
    }
}