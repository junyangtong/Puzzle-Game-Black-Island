using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public class FakeIKBone : MonoBehaviour
    {
    public Transform Bone;
    public Transform Target;

    // 骨骼转动速度
    public float Speed = 30;
    // 骨骼恢复速度
    public float RecoverSpeed = 20;
    // 世界坐标系的y轴
    private Vector3 upaxis { get { return Vector3.right; } }
    // 世界坐标系的上方向
    protected Vector3 transformUpAxis { get { return Bone.rotation * upaxis; } }
    // 本地坐标系的上方向
    protected Vector3 localTransformUpAxis { get { return Bone.localRotation * upaxis; } }
    // 当前转动四元数
    protected Quaternion currentQuaternion = Quaternion.identity;
    // 旋转函数
    public virtual void RotateToTarget(Vector3 targetPosition) 
    { 

    }
}