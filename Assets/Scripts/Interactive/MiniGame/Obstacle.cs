using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Vector3 direction; // 物体移动向量
    public float moveSpeed; // 物体移动速度

    private void Start()
    {
        // 获取一个随机方向，或者直接赋值某个方向都行。
        direction = new Vector3(0,-1,0);//GetRandomRotation()
    }

    void Update()
    {
        // 物体持续按给定的方向移动
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
    	// 检测碰到墙壁时改变物体原有移动方向
        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            Vector3 dir = Vector3.Reflect(direction, collision.GetContact(0).normal);
            //dir.y = 0;
            direction = dir;
        }
    }
    
    private Vector3 GetRandomRotation()
    {
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        return dir;
    }
    private void OnTriggerExit(Collider other) 
    {
        Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 10)
        { 
            //进入管道
            Debug.Log("进入管道，游戏结束");
            CatchFish.Instance.isLaunch = false;
            
        }
    }
}
