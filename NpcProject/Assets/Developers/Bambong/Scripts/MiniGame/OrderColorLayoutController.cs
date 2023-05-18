using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderColorLayoutController : MonoBehaviour
{
    [SerializeField]
    private GameObject colorKeyGo;
    [SerializeField]
    private GameObject arrowGo;

    public void Init(int[] keys , MiniGameManager manager)
    {
        for(int i = 0; i < keys.Length-1; ++i) 
        {
            var temp = Instantiate(colorKeyGo, transform);
            temp.GetComponent<Image>().color = manager.GetOrderKeyColor(keys[i]);
            Instantiate(arrowGo, transform);
        }
        var last = Instantiate(colorKeyGo, transform);
        last.GetComponent<Image>().color = manager.GetOrderKeyColor(keys[keys.Length - 1]);
    }

}
