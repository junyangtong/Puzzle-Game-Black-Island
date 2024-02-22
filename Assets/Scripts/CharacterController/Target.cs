using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Transform body;                  //身体
    public LayerMask terrainLayer;          //检测图层
    Vector3 newposition, oldposition, currentposition; //位置

    public float footSpacing; //落点偏移
    public float footSpacingFront; //落点偏移
    public float stepstance;                //步长
    //public float stepLength = 4;
    public float high = 0.1f;               //高度
    public float speed = 2;                 //速度
    //public Vector3 footOffset = default;
    float lerp = 1;

    public Target otherleg;                //约束

    private void OnEnable() 
    {
        EventHandler.OffsetCharacterTarget += OnOffsetCharacterTarget;
    }
    private void OnDisable() 
    {
        EventHandler.OffsetCharacterTarget -= OnOffsetCharacterTarget;
    }
    //运行时更新target偏移量 在playercontroller传参
    private void OnOffsetCharacterTarget(bool isMove)
    {
        if(isMove)
        {
            footSpacingFront = 0.1f;
        }
        else
        {
            footSpacingFront = -0.05f;
        }
    }
            
    private void Start()
    {
        newposition = transform.position;
        currentposition = transform.position;
    }

    void Update()
    {
        transform.position = currentposition;
        Ray ray = new Ray(body.position + (body.forward*footSpacingFront) + (body.right*footSpacing), Vector3.down);    // 设置射线发射方向
        if (Physics.Raycast(ray,out RaycastHit info, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newposition, info.point) > stepstance && !otherleg.IsMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newposition).z ? 1 : -1;
                newposition = info.point;// + (body.up * stepLength * direction) + footOffset;
            }
        }

        if (lerp < 1)
        {
            Vector3 footposition = Vector3.Lerp(oldposition, newposition, lerp);
            footposition.y += Mathf.Sin(lerp * Mathf.PI) * high;
            currentposition = footposition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldposition = newposition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newposition, 0.2f);
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }
}
