using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FakeHeadIKBone : FakeIKBone {
    // 头与观察点超过AngleMove之后，头开始转动
    public float AngleMove = 4;
    // 头一直可以转到AngleStay，之后保持直到超过AngleRecover
    public float AngleStay = 20;
    public float AngleRecover = 90;

    [HideInInspector]
    public bool InRecover;

    public override void RotateToTarget(Vector3 targetPosition) {
        if (Bone == null) return;
        //旋转目标向量
        Vector3 baseTargetDir = targetPosition - Bone.position;
        //当前骨骼上方向 与 目标向量的夹角
        float angle = Vector3.Angle(transformUpAxis, baseTargetDir);
        InRecover = false;
        //判断一下夹角
        /*if (angle <= AngleMove) {
            //啥也不干，如果发现不是正常角度，恢复到正常角度
            currentQuaternion = Quaternion.RotateTowards(currentQuaternion, Quaternion.identity, RecoverSpeed * Time.deltaTime);
        }*/
        //else if (angle > AngleMove && angle <= AngleRecover) {
            // 旋转轴
            Vector3 axisTemp = Vector3.Cross(transformUpAxis, baseTargetDir).normalized;
            float angleTemp = angle - AngleMove;
            //if (angleTemp > AngleStay - AngleMove) {
            //    //超出AngleStay，维持头的这个旋转
                angleTemp = AngleStay - AngleMove;
            //}
            
            //真正的旋转
            Quaternion quaternion = Quaternion.AngleAxis(angleTemp, axisTemp);
            
        //这里后面说，主要是做一个保持头部水平的矫正
            //Vector3 realBlue = quaternion * Bone.forward;
            //float realAngle = 90 - Vector3.Angle(realBlue, Vector3.up);
            
            //矫正Bone 的 蓝色轴，使之水平
            //Quaternion quaternionTemp = Quaternion.AngleAxis(CorrectAngle * axisTemp.y, Bone.up);
            //Quaternion quaternionCorrect = Quaternion.AngleAxis(realAngle, Bone.up);
            
            //quaternionCorrect * quaternion == 先转quaternion, 再转矫正quaternionCorrect 
            //quaternion = quaternionCorrect * quaternion;
            
            currentQuaternion = Quaternion.RotateTowards(currentQuaternion, quaternion, Speed * Time.deltaTime);
        //}
        //else if (angle > AngleRecover) {
            //恢复 到正常角度
        //    InRecover = true;
        //    currentQuaternion = Quaternion.RotateTowards(currentQuaternion, Quaternion.identity, RecoverSpeed * Time.deltaTime);
        //}

        Bone.rotation = currentQuaternion * Bone.rotation;
    }
    private void Update() {
        RotateToTarget(Target.position);
    }
    }