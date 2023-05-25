using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterKeywordStatusLayoutController : KeywordStatusLayoutController
{
    
    public override void Opne() 
    {
        //panel.gameObject.SetActive(true);
        debugIcon.gameObject.SetActive(true);
    }
    public override void Close() 
    {
        //panel.gameObject.SetActive(false);
        debugIcon.gameObject.SetActive(false);
    }

}
