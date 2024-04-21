using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRTSea : MonoBehaviour
{
    [SerializeField]
    private RenderTexture m_RenderTex;
    private Transform m_Target;
    [SerializeField]
    private bool m_ShowRT = false;

    void Awake()
    {
        Shader.SetGlobalTexture("_GlobalRipplesRT", m_RenderTex);
    }

    private void LateUpdate()
    {
        m_Target = GameObject.FindWithTag("Player").transform;
        transform.position = new Vector3(m_Target.transform.position.x, transform.position.y, m_Target.transform.position.z);
    }

    private void OnGUI()
    {
        if (m_ShowRT)
        {
            GUI.DrawTexture(new Rect(256, 0, 256, 256), m_RenderTex, ScaleMode.ScaleToFit, false, 1);
        }
    }
}
