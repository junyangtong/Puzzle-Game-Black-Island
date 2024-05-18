using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Tree : Interactive
{
    public GameObject twig;
    public GameObject twigOld;
    protected override void OnClickedAction()
    {
        //掉落树枝
        twig.SetActive(true);
        twigOld.SetActive(false);
    }
}
