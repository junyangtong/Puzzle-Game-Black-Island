using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Bird : Interactive
{
    public PlayableDirector BirdFromStoneToLight;
    public PlayableDirector BirdFromLightToGouzi;
    public PlayableDirector BirdFromGouziToLight;
    public PlayableDirector BirdFromGouziDisappear;
    private int Time = 0;
    public GameObject Seed;
    private bool StartTurnHead = false;
    private float randomTime;
    private Animator anim;

    private void Start() 
    {
        anim = this.GetComponent<Animator>();
    }
    protected override void OnClickedAction()
    {
        if(Time == 0)
        {
            // 第一次飞走
            Debug.Log("鸟第一次被赶走");
            FromStoneToLight();
            Time += 1;
            // 开始执行转头动画
            StartTurnHead = true;
        }
        else
        {
            // 第二次飞走
            Debug.Log("鸟第二次被赶走");
            FromGouziDisappear();
            // 掉落种子
            Seed.SetActive(true);
        }
        
    }
    private void Update() 
    {
        if (StartTurnHead)
        {
            RandomTurnHead();
        }
    }
    public void FromStoneToLight()
    {
        BirdFromStoneToLight.Play();
    }
    public void FromLightToGouzi()
    {
        BirdFromLightToGouzi.Play();;
    }
    public void FromGouziToLight()
    {
        BirdFromGouziToLight.Play();
    }
    public void FromGouziDisappear()
    {
        BirdFromGouziDisappear.Play();
    }
    /// <summary>
    /// 随机时间转头
    /// </summary>
    public void RandomTurnHead()
    {
        randomTime = Random.Range(0.5f, 2);
        Invoke("TurnHead", randomTime);
    }
    public void TurnHead()
    {
        anim.SetTrigger("TurnHead");
    }
}
