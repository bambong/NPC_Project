//using DG.Tweening;
//using MoreMountains.Feedbacks;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Searcher;
using UnityEngine;
//using static UnityEngine.Rendering.DebugUI.Table;
using FMODUnity;

public class KeywordGaugeItemController : MonoBehaviour , ISpawnAble
{

    [SerializeField]
    private GameObject keywordGo;
    [SerializeField]
    private Color baseColor = Color.white;
    [SerializeField]
    private Color myColor;

    [SerializeField]
    private float rotateSpeed = 100f;

    [SerializeField]
    private int gaugeAmount = 1;
    
    private bool isOn = true;

    private SpawnController parentSpawner;
    private Transform spawnSpot;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", myColor);
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", baseColor);
    }
    private void FixedUpdate()
    {
        var speed = rotateSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
        transform.Rotate(new Vector3(speed, speed, speed));
    }
    public void Init(SpawnController spawner)
    {
        var randoms = Random.Range(0, 360f);
        isOn = true;
        transform.rotation = Quaternion.Euler(randoms, randoms, randoms);
       
        parentSpawner = spawner;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isOn)
        {
            isOn = false;
            Managers.Effect.PlayEffect(Define.EFFECT.KeywordGaugeItemEffect,transform);
            Managers.Sound.PlaySFX(Define.SOUND.Item);
            Managers.Keyword.AddKeywordMakerGauge(keywordGo.GetComponent<KeywordController>(), gaugeAmount);
            if(parentSpawner != null) 
            {
                parentSpawner.RemoveItem(spawnSpot);
            }
            Managers.Resource.Destroy(gameObject);
        }
    }

    public void SetSpawnController(Transform spot, SpawnController controller)
    {
        transform.position = spot.position;
        spawnSpot = spot;
        Init(controller);
    }
}
