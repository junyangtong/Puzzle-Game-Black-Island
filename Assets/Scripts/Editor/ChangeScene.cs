
using UnityEngine;
using UnityEditor;

public class HorizontalGUI : EditorWindow
{
    [MenuItem("地编工具/地编工具")]
    public static void HorizontalGUIEnum()
    {
        EditorWindow.GetWindow<HorizontalGUI>(false, "地编工具");
    }
    /// <summary>
    /// 窗口内显示的GUI面板
    /// </summary>
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("快速切换时间线");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("时间线1", GUILayout.Height(30)))
        {
            ChangeToH1();
        }

        if (GUILayout.Button("时间线2", GUILayout.Height(30)))
        {
            ChangeToH2();
        }
        if (GUILayout.Button("时间线3", GUILayout.Height(30)))
        {
            ChangeToH3();
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("创建可拾取道具");
        GUILayout.EndHorizontal();
        if (GUILayout.Button("时间线3", GUILayout.Height(30)))
        {
            ChangeToH3();
        }
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("创建可交互物品");
        GUILayout.EndHorizontal();
        if (GUILayout.Button("时间线3", GUILayout.Height(30)))
        {
            ChangeToH3();
        }

    }
    static void ChangeToH1(){
		Transform[] CUs=Resources.FindObjectsOfTypeAll<Transform> ();
		foreach (Transform temp in CUs) {
			if (temp.name == "H1") {
				temp.gameObject.SetActive(true);
			}
            if (temp.name == "H2") {
				temp.gameObject.SetActive(false);
			}
            if (temp.name == "H3") {
				temp.gameObject.SetActive(false);
			}
		}
	}
    static void ChangeToH2(){
		Transform[] CUs=Resources.FindObjectsOfTypeAll<Transform> ();
		foreach (Transform temp in CUs) {
			if (temp.name == "H1") {
				temp.gameObject.SetActive(false);
			}
            if (temp.name == "H2") {
				temp.gameObject.SetActive(true);
			}
            if (temp.name == "H3") {
				temp.gameObject.SetActive(false);
			}
		}
	}
    static void ChangeToH3(){
		Transform[] CUs=Resources.FindObjectsOfTypeAll<Transform> ();
		foreach (Transform temp in CUs) {
			if (temp.name == "H1") {
				temp.gameObject.SetActive(false);
			}
            if (temp.name == "H2") {
				temp.gameObject.SetActive(false);
			}
            if (temp.name == "H3") {
				temp.gameObject.SetActive(true);
			}
		}
	}
}