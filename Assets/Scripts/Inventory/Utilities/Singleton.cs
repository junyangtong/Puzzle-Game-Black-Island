using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T>:MonoBehaviour where T:Component
{
    // 静态属性可全局访问
    public static T Instance {get; private set;}
    // 创建实例
    protected virtual void Awake(){
        Instance = this as T;
    }       
}