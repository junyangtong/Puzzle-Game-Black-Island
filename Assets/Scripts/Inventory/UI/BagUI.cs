using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    private Animation anim;
    private void Start() 
    {
        anim = GetComponent<Animation>();
    }
    public void BagClose()
    {
        anim.Play("BagClose");
    }
    public void BagOpen()
    {
        anim.Play("BagOpen");
    }
}
