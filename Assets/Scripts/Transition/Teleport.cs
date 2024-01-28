using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Teleport : MonoBehaviour
{
    public GameObject sceneFrom;

    public GameObject sceneToGo;

    public void TeleportToScene()
    {
        sceneToGo.SetActive(true);
        sceneFrom.SetActive(false);
    }
}
