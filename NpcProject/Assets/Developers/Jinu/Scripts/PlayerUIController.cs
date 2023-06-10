using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering;

public class PlayerUIController : UI_Base
{
    [SerializeField]
    private GameObject playerUIPanel;
    [SerializeField]
    private Image hpUI;
    [SerializeField]
    private Image hpIncreaseUI;
    [SerializeField]
    private Image hpDecreaseUI;
    [SerializeField]
    private Image hpDefalutUI;
    [SerializeField]
    private Image debugUI;
    [SerializeField]
    private CanvasGroup canvasGroup;

    private bool isDebugAni = false;
    private bool isDebugOpen = true;
    public override void Init()
    {
    }
   
    public void SetHp(int damage)
    {
        UIEnable();

        if (Managers.Game.Player.Hp <= Managers.Game.Player.MaxHp || Managers.Game.Player.Hp >= 0)
        {
            float exHp = (float)(Managers.Game.Player.Hp + damage) / Managers.Game.Player.MaxHp;
            float hp = (float)Managers.Game.Player.Hp / Managers.Game.Player.MaxHp;

            if(hp >= 1)
            {
                hp = 1;
            }

            FillUI(exHp);

            if (damage > 0)
            {
                HpUIAnimation(hpDecreaseUI, hp);
            }
            else
            {
                HpUIAnimation(hpIncreaseUI, hp);
            }
        }
    }

    private void FillUI(float exHp)
    {
        hpUI.fillAmount = exHp;
        hpIncreaseUI.fillAmount = exHp;
        hpDecreaseUI.fillAmount = exHp;
    }

    private void HpUIAnimation(Image hpChangeUI, float hp)
    {
        hpUI.DOFillAmount(hp, 0.1f).OnComplete(() =>
        {
            hpChangeUI.DOFillAmount(hp, 0.5f).OnComplete(() =>
            {
                hpChangeUI.gameObject.SetActive(false);
                hpUI.DOFade(0f, 1.0f).OnStart(() =>
                {
                    //canvasGroup.DOFade(0f, 1f);
                    hpDefalutUI.DOFade(0f, 1.0f);
                }).OnComplete(() =>
                {
                    hpUI.gameObject.SetActive(false);
                    hpDefalutUI.gameObject.SetActive(false);
                });
            });
        });
    }

    private void UIEnable()
    {
        hpDefalutUI.gameObject.SetActive(false);

        hpUI.DOKill();
        hpDefalutUI.DOKill();
        hpDecreaseUI.DOKill();
        canvasGroup.DOKill();
        hpUI.gameObject.SetActive(true);
        hpDefalutUI.gameObject.SetActive(true);
        hpDecreaseUI.gameObject.SetActive(true);
        //canvasGroup.DOFade(1.0f, 0.2f);
        hpUI.DOFade(1.0f, 0f);
        //hpUI.DOFade(1.0f, 0f);
         hpDefalutUI.DOFade(1.0f, 0f);
    }

    public void DebugButton()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if(Managers.Keyword.CurDebugZone == null || !Managers.Keyword.CurDebugZone.IsDebugAble)
        {
            isDebugOpen = false;
            debugUI.DOKill();
            debugUI.gameObject.SetActive(false);
        }
        else
        {
            isDebugOpen = true;
            isDebugAni = true;
            var color = debugUI.color;
            color.a = 0;
            debugUI.color = color;
            debugUI.gameObject.SetActive(true);
            StartCoroutine(OpenDebugUI());
        }
    }


    IEnumerator OpenDebugUI()
    {
        while(isDebugOpen)
        {   
            if (isDebugAni)
            {
                debugUI.DOFade(0f, 1.0f).OnStart(() =>
                {
                    isDebugAni = false;
                }).OnComplete(() =>
                {
                    isDebugAni = true;
                    debugUI.DOFade(1.0f, 0);
                });
            }    
            if(Managers.Game.IsDebugMod)
            {
                debugUI.DOKill();
                debugUI.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }        
    }
}
