using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PlayerController))]
public class InspectorTestEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Click Me"))
        {
            //Logic
            
        }
    }
}