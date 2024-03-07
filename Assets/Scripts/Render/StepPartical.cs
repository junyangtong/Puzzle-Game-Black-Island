using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPartical : MonoBehaviour
{
    private ParticleSystem step;
    private void Awake() 
    {
        step = this.gameObject.GetComponent<ParticleSystem>();
    }
    public void StarDrawStep()
    {
        step.Play();
        Debug.Log("开始画脚印");
    }
    public void StopDrawStep()
    {
        step.Stop();
        Debug.Log("停止画脚印");
    }
}
