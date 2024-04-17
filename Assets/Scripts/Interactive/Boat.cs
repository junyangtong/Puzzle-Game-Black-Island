using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : Interactive
{
    public Transform InBoatPos;
    public Transform OutBoatPos;
    public GameObject InBoatToolTip;
    
    protected override void OnClickedAction()
    {
        TeleportToBoat();
    }
    public override void EmptyClicked()
    {
        
    }
    private void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Player")
        {
            InBoatToolTip.SetActive(true);
        }
    }
    public void TeleportToBoat()
    {
        if(InBoatPos != null)
        {
            EventHandler.CallTeleportEvent(InBoatPos.position);
        }
        else
            Debug.LogWarning("未选择角色瞬移目的地！！");
    }
    public void TeleportOutBoat()
    {
        if(OutBoatPos != null)
        {
            EventHandler.CallTeleportEvent(OutBoatPos.position);
        }
        else
            Debug.LogWarning("未选择角色瞬移目的地！！");
    }
}