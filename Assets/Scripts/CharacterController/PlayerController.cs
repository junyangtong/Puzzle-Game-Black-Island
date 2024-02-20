using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3;
    public float rotateLerp = 0.1f; // 旋转插值比例
    private CharacterController car;
    //private Animator anim;
    private Transform player;
    private Vector3 targetDirection,currentDirection;
    private bool canMove = false;
    public BodyWave bodyWave;
    private void OnEnable() 
    {
        EventHandler.GameStateChangeEvent += OnGameStateChangeEvent;
    }
    private void OnDisable() 
    {
        EventHandler.GameStateChangeEvent -= OnGameStateChangeEvent;
    }
    private void OnGameStateChangeEvent(GameState gameState)
    {
        canMove = gameState == GameState.MiniGame;
    }
    void Start()
    {
        car = GetComponent<CharacterController>();
        //anim = GetComponent<Animator>();
        player = this.transform;
    }

    void Update()
    {   
        // 角色移动
        Vector3 move = Vector3.zero;
        if(!canMove)
        {
            move.x = Input.GetAxis("Horizontal") * Time.deltaTime;
            move.z = Input.GetAxis("Vertical") * Time.deltaTime;
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
            targetDirection = new Vector3(-move.x,0f,-move.z);
            //anim.SetBool("isMoving",true);
            bodyWave.startWave = true;
        }
        else
        {
            //anim.SetBool("isMoving",false);
            targetDirection = currentDirection;
            bodyWave.startWave = false;
        }
        if(!canMove)
        {
        //float rotationAngle = Mathf.Acos(Vector3.Dot(targetDirection, currentDirection)) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), rotateLerp);
        //transform.LookAt(transform.position + targetDirection); 
        }
    }
}