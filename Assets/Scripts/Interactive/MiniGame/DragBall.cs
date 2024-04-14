
using UnityEngine;
using System.Collections;

public class DragBall : MonoBehaviour
{
    public Camera cam;
     //偏移值
    Vector3 m_Offset;
    //当前物体对应的屏幕坐标
    Vector3 m_TargetScreenVec;

    private IEnumerator OnMouseDown()
    {
        //当前物体对应的屏幕坐标
        m_TargetScreenVec = cam.WorldToScreenPoint(transform.position);
        //偏移值=物体的世界坐标，减去转化之后的鼠标世界坐标（z轴的值为物体屏幕坐标的z值）
        m_Offset = transform.position - cam.ScreenToWorldPoint(new Vector3
        (Input.mousePosition.x, Input.mousePosition.y, m_TargetScreenVec.z));
        //当鼠标左键点击
        while (Input.GetMouseButton(0))
        {
            //当前坐标等于转化鼠标为世界坐标（z轴的值为物体屏幕坐标的z值）+ 偏移量
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, m_TargetScreenVec.z)) + m_Offset;
            //等待固定更新
            yield return new WaitForFixedUpdate();
        }
    }
}
