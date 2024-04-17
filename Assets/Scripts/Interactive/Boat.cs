using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : Interactive
{
    public Transform InBoatPos;
    public Transform OutBoatPos;
    public GameObject InBoatToolTip;

    private GameObject Player;
    private PlayerController playerController;
    
    protected override void OnClickedAction()
    {
        //TeleportToBoat();
    }
    public override void EmptyClicked()
    {
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            Player = other.gameObject;
            playerController = Player.GetComponent<PlayerController>();
            InBoatToolTip.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        Player = null;
        InBoatToolTip.SetActive(false);
    }
    public void TeleportToBoat()
    {
        if(InBoatPos != null && Player != null)
        {
            playerController.enabled = false;
            Player.transform.position = InBoatPos.position;
            playerController.enabled = true;
        }
        else
            Debug.LogWarning("未选择角色瞬移目的地或角色不在范围内！！");
    }
    public void TeleportOutBoat()
    {
        if(OutBoatPos != null && Player != null)
        {
            playerController.enabled = false;
            Player.transform.position = OutBoatPos.position;
            playerController.enabled = true;
        }
        else
            Debug.LogWarning("未选择角色瞬移目的地或角色不在范围内！！");
    }
}