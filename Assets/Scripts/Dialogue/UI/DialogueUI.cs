using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueUI : MonoBehaviour
{
    public GameObject panel;
    public Text dialogueText;
    public bool isFollow = true;
    private RectTransform rectTransform;
    private Vector3 offset;
    private Vector3 m_Target;

    private void OnEnable() 
    {
        EventHandler.ShowDialogueEvent += ShowDialogue;
    }
    private void OnDisable() 
    {
        EventHandler.ShowDialogueEvent -= ShowDialogue;
    }
    private void ShowDialogue(string dialogue)
    {
        if(dialogue != string.Empty)
            panel.SetActive(true);
        else
            panel.SetActive(false);
        dialogueText.text = dialogue;
    }
    private void Awake() 
    {
        m_Target = GameObject.FindWithTag("Player").transform.position;
    }
    private void Start() 
    {
        rectTransform = panel.GetComponent<RectTransform>();
        // 设置相对偏移
        offset = m_Target - rectTransform.anchoredPosition3D;
    }
    private void Update() 
    {
        if(isFollow)
        {
            // 对话框跟随主角移动
            // 更新Player
            m_Target = GameObject.FindWithTag("Player").transform.position;
            // 更新位置
            rectTransform.anchoredPosition3D = m_Target - offset; //= new Vector3(posx,posy,posz)
        }
        
    }
}
