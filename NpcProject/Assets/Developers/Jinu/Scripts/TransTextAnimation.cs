using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class TransTextAnimation : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI outputText;
    [SerializeField]
    private Image image;
    [SerializeField]
    private string[] changetext;    
    [SerializeField]
    private UnityEvent completEvent;


    private void Start()
    {
        Invoke("TransText", 0.5f);
    }
    private void TransText()
    {
        StartCoroutine(Trans(changetext));
    }
    
    IEnumerator Trans(string[] text)
    {
        char[] sep = { '#', '#' };

        for(int i = 0; i < text.Length; i++)
        {
            string[] result = text[i].Split(sep);
            text[i] = "";

            foreach(var item in result)
            {
                if(item == "player")
                {
                    text[i] += Managers.Talk.GetSpeakerName(101);
                    continue;
                }
                text[i] += item;
            }

            for (int j = 0; j < text[i].Length; j++)
            {
                outputText.text = text[i].Substring(0, j + 1) + RandomText(text[i].Length - j - 1);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1.0f);
        }
        outputText.text = "";
        image.DOFade(0.0f, 0.5f).OnComplete(()=>
        {
            completEvent?.Invoke();
        });
    }

    private string RandomText(int length)
    {
        var random = new System.Random();
        string charcters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return "<color=white>" + new string(Enumerable.Repeat(charcters, length).Select(s => s[random.Next(s.Length)]).ToArray()) + "</color>";
    }
}
