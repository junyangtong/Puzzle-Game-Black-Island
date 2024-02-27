using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyWave : MonoBehaviour
{
    Vector3 trans1;//记录原位置
    Vector3 trans2;//简谐运动变化的位置，计算得出
    public bool startWave;
    public float amplitude = 10f;//振幅
    public float frequency = 1f;//频率

    private void OnEnable() 
    {
        EventHandler.OffsetCharacterTarget += OnOffsetCharacterTarget;
    }
    private void OnDisable() 
    {
        EventHandler.OffsetCharacterTarget -= OnOffsetCharacterTarget;
    }
    //运行时更新target偏移量 在playercontroller传参
    private void OnOffsetCharacterTarget(bool isMove)
    {
        startWave = isMove;
    }
    private void Awake()
    {
        startWave = false;
        trans1 = transform.localPosition;
    }

    private void Update()
    {
        if(startWave)
        {
            trans2 = trans1;
            trans2.y = Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude + trans1.y;

            transform.localPosition = trans2;
        }
        
    }
}
