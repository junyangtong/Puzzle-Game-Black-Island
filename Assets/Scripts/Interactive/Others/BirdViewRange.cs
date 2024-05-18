using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdViewRange : MonoBehaviour
{
    public Bird bird;
    public PlayerController playerController;

    private void OnTriggerEnter(Collider other) 
    {
        // 如果检测到主角,主角正在移动，则鸟飞回路灯
        if(other.gameObject.tag == "Player")
        {
            if (playerController.isMove)
            {
                bird.FromGouziToLight();
            }
        }
    }
}
