using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform m_Target;
    private Vector3 offset;
    [Header("默认FOV")]
    public float Fov;
    [Header("变焦时的最大FOV")]
    public float FovTarget;
    [Header("变焦速度")]
    public float FovSpeed;

    private void Awake() 
    {
        m_Target = GameObject.FindWithTag("Player").transform;
    }
    private void Start()
    {
        // 设置相对偏移
        offset = m_Target.position - this.transform.position;
    }
    private void Update()
    {   
        // 更新Player
        m_Target = GameObject.FindWithTag("Player").transform;
        // 更新位置
        this.transform.position = m_Target.position - offset;
        
        // 丝滑变焦
        Camera.main.fieldOfView = Fov;
        if(Fov > FovTarget)
            Fov -= FovSpeed * Time.deltaTime;
    }
}
