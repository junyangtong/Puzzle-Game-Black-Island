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
    public GameObject birdViewRange;
    public GameObject Seed;
    private bool StartTurnHead = false;
    private bool isTurnning = false;
    private bool fistTurn = false;
    private float randomTime;
    private Animator anim;
    public Animator birdViewRangeAnim;
    

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
        }
        else
        {
            // 第二次飞走
            Debug.Log("鸟第二次被赶走");
            FromGouziDisappear();
            // 掉落种子
            Seed.SetActive(true);
            // 结束执行转头动画
            isTurnning = false;
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
        BirdFromLightToGouzi.Play();
        // 开始执行转头动画
        StartTurnHead = true;
        fistTurn = true;
    }
    public void FromGouziToLight()
    {
        BirdFromGouziToLight.Play();
        // 结束执行转头动画
        StartTurnHead = false;
    }
    public void FromGouziDisappear()
    {
        BirdFromGouziDisappear.Play();
        birdViewRange.SetActive(false);
    }
    /// <summary>
    /// 随机时间转头
    /// </summary>
    public void RandomTurnHead()
    {
        if (!isTurnning)
        {
            isTurnning = true;
            randomTime = Random.Range(1f, 2.5f);
            if(fistTurn)
            {
                Invoke("TurnHead", 0.7f);
            }
            else
            {
                Invoke("TurnHead", randomTime);
            }
        }
        
    }
    private void TurnHead()
    {
        anim.SetTrigger("TurnHead");
        birdViewRangeAnim.SetTrigger("TurnHead");
        isTurnning = false;
        fistTurn = false;
    }
}
