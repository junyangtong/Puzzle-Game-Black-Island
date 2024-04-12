
using UnityEngine;
using System.Collections;
 
public class DragBall : MonoBehaviour
{
    /// <summary>
    /// 将要拖动的物体
    /// </summary>
    private GameObject _dragGameObject;
 
    /// <summary>
    /// 获取射线需要碰撞的层
    /// </summary>
    private LayerMask _canDrag;
 
    /// <summary>
    /// 直接从外部定义好层，简单理解
    /// </summary>
    public LayerMask canDrag2;
 
    /// <summary>
    /// 获得鼠标的位置和cube位置差
    /// </summary>
    private Vector3 _offset;
 
    /// <summary>
    /// 是否点击到cube
    /// </summary>
    private bool _isClickCube;
 
    /// <summary>
    /// 目标对象的屏幕坐标
    /// </summary>
    private Vector3 _targetScreenPoint;
 
    //限制拖动范围的最小值和最大值
    public float xMin, xMax, zMin, zMax;
 
    // Use this for initialization
    private void Start()
    {
        _canDrag = 1 << LayerMask.NameToLayer("Player");
    }
 
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckGameObject())
            {
                _offset = _dragGameObject.transform.position -
                          Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                              _targetScreenPoint.z));
            }
        }
 
        if (_isClickCube)
        {
            //当前鼠标所在的屏幕坐标
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _targetScreenPoint.z);
            //把当前鼠标的屏幕坐标转换成世界坐标
            Vector3 curWorldPoint = Camera.main.ScreenToWorldPoint(curScreenPoint);
            _dragGameObject.transform.position = curWorldPoint + _offset;
 
            //限制拖动范围
            _dragGameObject.GetComponent<Rigidbody>().transform.position = new Vector3(
                Mathf.Clamp(_dragGameObject.GetComponent<Rigidbody>().transform.position.x, xMin, xMax),
                1.0f,
                Mathf.Clamp(_dragGameObject.GetComponent<Rigidbody>().transform.position.z, zMin, zMax)
            );
        }
 
        if (Input.GetMouseButtonUp(0))
        {
            _isClickCube = false;
            //恢复拖拽物体的Y轴为原点
            //_dragGameObject.GetComponent<Rigidbody>().transform.position = new Vector3(
             //   _dragGameObject.GetComponent<Rigidbody>().transform.position.x, 0,
             //   _dragGameObject.GetComponent<Rigidbody>().transform.position.z);
        }
    }
 
    /// <summary>
    /// 检查是否点击到cbue
    /// </summary>
    /// <returns></returns>
    private bool CheckGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f, _canDrag))
        {
            _isClickCube = true;
            //得到射线碰撞到的物体
            _dragGameObject = hitInfo.collider.gameObject;
            _targetScreenPoint = Camera.main.WorldToScreenPoint(_dragGameObject.transform.position);
            Debug.Log("点到球");
            return true;
        }
 
        return false;
    }
}
