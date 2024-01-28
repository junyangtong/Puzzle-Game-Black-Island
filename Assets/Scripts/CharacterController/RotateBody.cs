using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBody : MonoBehaviour
{
    void LateUpdate()
    {
        RaycastHit hit;     //射线
        int Rmask = LayerMask.GetMask("ik");
        Vector3 Point_dir = Vector3.down;
        if (Physics.Raycast(transform.position, Point_dir, out hit, 50.0f, Rmask))
        {
            //打印一条从物体到碰撞点的红色射线，hit.point世界空间中射线命中碰撞体的撞击点
            Debug.DrawLine(transform.position, hit.point, Color.red);
            Quaternion NextRot = Quaternion.LookRotation(Vector3.Cross(hit.normal, Vector3.Cross(transform.forward, hit.normal)), hit.normal);
            //GetComponent<Rigidbody>().MoveRotation(Quaternion.Lerp(transform.rotation, NextRot, 0.1f)); //旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, NextRot, 0.1f);
        }

    }
}
