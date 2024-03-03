
using UnityEngine;
using UnityEditor;

public class HorizontalGUI : EditorWindow
{
    [MenuItem("地编工具/快速切换时间线")]
    public static void HorizontalGUIEnum()
    {
        EditorWindow.GetWindowWithRect<HorizontalGUI>(new Rect(0, 0, 500, 30), false, "切换时间线");
    }
    /// <summary>
    /// 窗口内显示的GUI面板
    /// </summary>
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("时间线1"))
        {
            ChangeToH1();
        }

        if (GUILayout.Button("时间线2"))
        {
            ChangeToH2();
        }
        if (GUILayout.Button("时间线3"))
        {
            ChangeToH3();
        }

        GUILayout.EndHorizontal();
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