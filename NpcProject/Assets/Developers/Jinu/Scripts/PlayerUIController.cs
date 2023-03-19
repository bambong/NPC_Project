using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    private bool isHpOpen = true;
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
        StartCoroutine(OpenhpUI());
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
                    hpDefalutUI.DOFade(0f, 1.0f);
                }).OnComplete(() =>
                {
                    isHpOpen = false;
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
        hpUI.gameObject.SetActive(true);
        hpDefalutUI.gameObject.SetActive(true);
        hpDecreaseUI.gameObject.SetActive(true);
        hpUI.DOFade(1.0f, 0f);
        hpDefalutUI.DOFade(1.0f, 0f);
    }

    public void DebugButton()
    {
        if(Managers.Keyword.CurDebugZone == null)
        {
            isDebugOpen = false;
            debugUI.DOKill();
            debugUI.gameObject.SetActive(false);
        }
        else
        {
            isDebugOpen = true;
            isDebugAni = true;
            debugUI.gameObject.SetActive(true);
            StartCoroutine(OpenDebugUI());
        }
    }

    IEnumerator OpenhpUI()
    {
        isHpOpen = true;
        while (isHpOpen)
        {
            playerUIPanel.transform.position = Managers.Game.Player.transform.position + (Vector3.up * 4);
            playerUIPanel.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            yield return null;
        }
    }

    IEnumerator OpenDebugUI()
    {
        while(isDebugOpen)
        {
            playerUIPanel.transform.position = Managers.Game.Player.transform.position + (Vector3.up * 4);
            playerUIPanel.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);            
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
            if(Managers.Game.Player.IsDebugMod)
            {
                debugUI.DOKill();
                debugUI.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }        
    }
}
