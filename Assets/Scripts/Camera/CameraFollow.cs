using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform m_Target;
    private Vector3 offset;
    private void Awake() 
    {
        m_Target = GameObject.FindWithTag("Player").transform;
    }
    private void Start()
    {
        //设置相对偏移
        offset = m_Target.position - this.transform.position;
    }
    private void Update()
    {   
        //更新Player
        m_Target = GameObject.FindWithTag("Player").transform;
        //更新位置
        this.transform.position = m_Target.position - offset;
    }
}
