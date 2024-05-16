using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P0ToP1 : MonoBehaviour
{
    public LoadManager loadManager;
    private void OnEnable() 
    {
        loadManager.LoadPart1();
    }
}
