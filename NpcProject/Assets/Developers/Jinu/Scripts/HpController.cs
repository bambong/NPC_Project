using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpController : UI_Base
{
    [SerializeField]
    private Image hp1;
    [SerializeField]
    private Image hp2;
    [SerializeField]
    private Image hp3;
    [SerializeField]
    private Image hp4;

    private List<Image> hpBar = new List<Image>();
    
    public override void Init()
    {
        hpBar.Add(hp1);
        hpBar.Add(hp2);
        hpBar.Add(hp3);
        hpBar.Add(hp4);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void GetDamage()
    {
        int curHp = Managers.Game.Player.Hp;
        if(curHp >= 0)
        {
            hpBar[curHp].gameObject.SetActive(false);
        }
    } 

    public void GetHp()
    {
        int curHp = Managers.Game.Player.Hp - 1;
        Debug.Log(curHp);
        if(curHp <= hpBar.Count)
        {
            Debug.Log(curHp);
            hpBar[curHp].gameObject.SetActive(true);
        }
    }
}
