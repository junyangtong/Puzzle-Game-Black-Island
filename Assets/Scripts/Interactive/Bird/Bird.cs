using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Interactive
{
    private Animation anim;
    public int Time = 0;
    public GameObject Seed;
    private void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }
    protected override void OnClickedAction()
    {
        if(Time == 0)
        {
            // 第一次飞走
            Debug.Log("鸟被赶走");
            anim.Play("BirdOnceTouch");
            Time += 1;
        }
        else
        {
            // 第二次飞走
            Debug.Log("鸟被赶走");
            anim.Play("BirdTwiceTouch");
            // 掉落种子
            Seed.SetActive(true);
        }
        
    }
}
