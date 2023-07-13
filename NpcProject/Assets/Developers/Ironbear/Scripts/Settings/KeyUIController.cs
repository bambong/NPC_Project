using UnityEngine;
using TMPro;

public class KeyUIController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] btnTxt;

    void Start()
    {
        for(int i=0; i< btnTxt.Length; i++)
        {
            btnTxt[i].text = KeySetting.keys[(KEY_TYPE)i].ToString();
        }        
    }

    void Update()
    {
        for (int i = 0; i < btnTxt.Length; i++)
        {
            btnTxt[i].text = KeySetting.keys[(KEY_TYPE)i].ToString();
        }
    }
}
