using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSpots : Interactive
{
    public GameObject Fish;
    public bool choosePos = false;
    public GameObject choosePosTarget;
    public GameObject yupiao;
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
        choosePos = true;
        // 出现 鱼
        Fish.SetActive(true);
        dialogueController.ShowdialogueFinish();
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
            yupiao.SetActive(true);
            yupiao.transform.position = choosePosTarget.transform.position;
            Debug.Log("释放鱼漂");
        }
    }
    void Update() 
    {
        if(choosePos)
            ChooseFishingPos();
    }
}
