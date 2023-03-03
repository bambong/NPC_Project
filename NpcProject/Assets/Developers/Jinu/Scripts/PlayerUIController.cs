using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : UI_Base
{
    [SerializeField]
    private Image hpUI;
    [SerializeField]
    private Image debugUI;

    public override void Init()
    {

    }

    public void SetHp()
    {
        
        if(Managers.Game.Player.Hp <= Managers.Game.Player.MaxHp)
        {
            if(Managers.Game.Player.Hp >= 0)
            {
                float hp = (float)Managers.Game.Player.Hp / Managers.Game.Player.MaxHp;
                hpUI.fillAmount = hp;
            }
        }
    }

    public void DebugButtom()
    {
        if(Managers.Keyword.CurDebugZone == null)
        {
            debugUI.gameObject.SetActive(false);
        }
        else
        {
            debugUI.gameObject.SetActive(true);
        }
    }
    public void OnPlayerUI()
    {
        this.gameObject.SetActive(true);
        if(Managers.Keyword.CurDebugZone == null)
        {
            debugUI.gameObject.SetActive(false);
        }
        else
        {
            debugUI.gameObject.SetActive(true);
        }
    }
    public void OffPlayerUI()
    {
        this.gameObject.SetActive(false);
    }
}
