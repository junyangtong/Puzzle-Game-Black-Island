using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedBox : Interactive
{
    public GameObject miniGameSceneToGo;
    public GameObject PerPassword;
    public float[] Password = new float[4]{2,2,2,2};
    private float[] currentPassword = new float[4]{0,0,0,0};
    private bool[] isEquit = new bool[4]{false,false,false,false};
    private float loopCount;
    private float amount;
    
    private void OnEnable() 
    {
        EventHandler.CheckGameStateEvent += OnCheckGameStateEvent;
    }
    void OnDisable()
    {
        EventHandler.CheckGameStateEvent -= OnCheckGameStateEvent;
    }
    private void OnCheckGameStateEvent()
    {
        for(int i = 0; i <= 3; i++)
        {
            var newLockedBox = PerPassword.transform.GetChild(i).GetComponent<PerPassword>();
            amount = newLockedBox.amount;
            loopCount = newLockedBox.loopCount;
            currentPassword[i] = newLockedBox.currentPassword;
            
            //需要减去递增的周期
            isEquit[i] = ((currentPassword[i]-amount*loopCount) == Password[i]) ? true : false;
            Debug.Log(currentPassword[i]-amount*loopCount);
            
        }
        if(isEquit[1]&&isEquit[2]&&isEquit[3]&&isEquit[0])
        {
            Debug.Log("密码正确");
        }
    }
    private void Awake()
    {
        
    }
    
    protected override void OnClickedAction()
    {
        miniGameSceneToGo.SetActive(true);
        EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
    }
    public override void EmptyClicked()
    {
        
    }
    public void Close()
    {
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        miniGameSceneToGo.SetActive(false);
    }
}
