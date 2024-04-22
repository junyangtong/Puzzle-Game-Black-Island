using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 如果是主角进入触发器 则告诉主角已经进入水中
        if(other.gameObject.tag == "Player")
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            player.isInWater = true;
            //player.SwitchinWater();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // 如果是主角进入触发器 则告诉主角已经进入水中
        if(other.gameObject.tag == "Player")
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            player.isInWater = false;
            //player.SwitchinGround();
        }
    }
}
