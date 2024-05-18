using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public float rotateLerp = 0.1f; // 旋转插值比例
    public GameObject StepParticall;
    public GameObject StepParticalr;
    public bool isInWater;
    public GameObject RippleEffect;
    public GameObject Rig;
    public GameObject FishRod;

    private CharacterController car;
    private Animator anim;
    private Transform player;
    private Vector3 targetDirection,currentDirection;
    private bool canMove = false;
    public bool isMove = false;
    private bool isRepulsed = false;
    private Vector3 RepulsedTarget;

    //计时器
    TimerMgr timer;
    int TimerID;
    
    private void OnEnable() 
    {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
        EventHandler.ItemCheckAnim += OnItemCheckAnim;
        EventHandler.StartFishing += OnStartFishing;
    }
    private void OnDisable() 
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
        EventHandler.ItemCheckAnim -= OnItemCheckAnim;
        EventHandler.StartFishing -= OnStartFishing;
    }
    private void OnGameStateChangeEvent(GameState gameState)
    {
        canMove = gameState == GameState.MiniGame;
    }
    private void OnItemCheckAnim(bool ItemCorrectly)
    {
        if(ItemCorrectly)
            anim.SetTrigger("ItemCorrectly");
        else
            anim.SetTrigger("ItemFalse");
    }
    private void OnStartFishing(bool isFishing)
    {
        if(isFishing)
        {
            anim.SetTrigger("Fishing");
            FishRod.SetActive(isFishing);
        }
        else
        {
            anim.SetTrigger("FishingOver");
            FishRod.SetActive(isFishing);
        }
            
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
                Vector3 RepulsedVector = (RepulsedTarget).normalized;
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
        anim.SetBool("inWater",isInWater);
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
    /// <summary>
    /// 碰撞到刚体返回hit
    /// </summary>
    /// <param name="hit"></param>
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