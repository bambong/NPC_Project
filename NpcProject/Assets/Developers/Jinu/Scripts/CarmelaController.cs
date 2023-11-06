using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmelaController : MonoBehaviour
{
    [SerializeField]
    private GameObject carmela;
    [SerializeField]
    private float timeInterval;
    [SerializeField]
    private float speed;

    [ContextMenu("ActiveCarmela")]
    public void ActiveCharacter()
    {
        Material mat = carmela.GetComponent<SpriteRenderer>().material;
        var hologramvalue = mat.GetFloat("_HologramBlend");

        StartCoroutine(ChangeValue(mat, hologramvalue));

        //mat.SetFloat("_HologramBlend", hologramvalue);        
    }

    IEnumerator ChangeValue(Material mat, float value)
    {
        var time = new WaitForSeconds(timeInterval);
        while (value > 0.1)
        {
            value = Mathf.Lerp(value, 0, Time.deltaTime * speed) ;
            mat.SetFloat("_HologramBlend", value);
            yield return time;
        }
        mat.SetFloat("_HologramBlend", 0);
    }
}
