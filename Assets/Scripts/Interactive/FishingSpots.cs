using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpots : Interactive
{
    public GameObject Fish;
    public GameObject FishUI;
    public bool choosePos = false;
    public GameObject choosePosTarget;
    public GameObject yupiao;
    private GameObject yupiaotemp;
    private DialogueController dialogueController;
    private RaycastHit hit;
    private Ray ray;
    private int i = 0;
    
    private void Awake()
    {
        Fish.SetActive(false);
        dialogueController = GetComponent<DialogueController>();
    }
    
    protected override void OnClickedAction()
    {
        // 播放使用物品的动画
        Debug.Log("开始钓鱼");
        EventHandler.CallStartFishing(true);
        choosePos = true;
        // 出现 鱼
        //Fish.SetActive(true);
        EventHandler.CallGameStateChangeEvent(GameState.MiniGame);
        //dialogueController.ShowdialogueFinish();
    }
    public void StopFish()
    {
        Debug.Log("收杆");
        EventHandler.CallGameStateChangeEvent(GameState.GamePlay);
        FishUI.SetActive(false);
        // 播放收起鱼漂的动画
        yupiaotemp.GetComponent<Yupiao>().PickYupiao();
        // 如果收杆时鱼漂抓到鱼，则显示鱼
        if(yupiaotemp.GetComponent<Yupiao>().Catching)
        {
            Fish.SetActive(true);
        }
        EventHandler.CallStartFishing(false);
    }
    public override void EmptyClicked()
    {
        if(!isDone)
            dialogueController.ShowdialogueFinish();
        else
            dialogueController.ShowdialogueEmpty();
    }
    // 选择鱼漂位置
    private void ChooseFishingPos()
    {
        // 向鼠标点击的位置发射射线
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 返回碰撞点位置
        if (Physics.Raycast(ray, out hit))
        {   
            choosePosTarget.SetActive(true);
            Debug.Log("选择位置");
            choosePosTarget.transform.position = hit.point;
        }
        // 再次点击释放鱼漂
        if (Input.GetMouseButtonDown(0))
        {
            i+=1;
        }
        if (i > 1)
        {
            choosePosTarget.SetActive(false);
            choosePos = false;
            FishUI.SetActive(true);
            //yupiao.SetActive(true);
            yupiaotemp = Instantiate(yupiao, choosePosTarget.transform.position, Quaternion.LookRotation(new Vector3(0,0,0)));
            yupiaotemp.transform.SetParent(this.transform); 
            //yupiao.transform.position = choosePosTarget.transform.position;
            Debug.Log("释放鱼漂");
            i = 0;
        }
    }
    void Update() 
    {
        if(choosePos)
            ChooseFishingPos();
    }
}
