using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIController : UI_Base
{
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
    private bool isHpAni = false;
    private bool isHpOpen = true;
    private bool isDebugAni = false;
    private bool isDebugOpen = true;
    private Vector3 hpUIPos;
    public override void Init()
    {
    }

    public void SetHp()
    {
        UIEnable();

        if (Managers.Game.Player.Hp <= Managers.Game.Player.MaxHp || Managers.Game.Player.Hp >= 0)
        {
            float hp = (float)Managers.Game.Player.Hp / Managers.Game.Player.MaxHp;
            //hpUI.fillAmount = hp;
            hpUI.DOFillAmount(hp, 0.1f).OnComplete(() =>
            {
                hpDecreaseUI.DOFillAmount(hp, 0.5f).OnComplete(() =>
                {
                    hpDecreaseUI.gameObject.SetActive(false);
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
        StartCoroutine(OpenhpUI());
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
            hpUIPos = Managers.Game.Player.transform.position + (Vector3.up * 4) + (hpUI.transform.rotation * Vector3.left * 0.4f);
            hpUI.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            hpUI.transform.position = hpUIPos;
            hpDecreaseUI.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            hpDecreaseUI.transform.position = hpUIPos;
            hpDefalutUI.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            hpDefalutUI.transform.position = hpUIPos;
            yield return null;
        }
    }

    IEnumerator OpenDebugUI()
    {
        while(isDebugOpen)
        {
            debugUI.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            debugUI.transform.position = Managers.Game.Player.transform.position + (Vector3.up * 4) +  (debugUI.transform.rotation * Vector3.left * -0.4f);
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
