using UnityEngine;
using TMPro;
using DG.Tweening;

public class KeyUIController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] btnTxt;

    void Start()
    {
        for(int i=0; i< btnTxt.Length; i++)
        {
            btnTxt[i].text = KeySetting.Instance.defaultKeys[(KEY_TYPE)i].ToString();
        }
    }

    void Update()
    {
        for (int i = 0; i < btnTxt.Length; i++)
        {
            btnTxt[i].text = KeySetting.Instance.tempKeys[(KEY_TYPE)i].ToString();
        }
    }
}
