using DG.Tweening;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnergeItemController : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 100f;

    [SerializeField]
    private int gaugeAmount = 1;

    private ItemSpawnController parentSpawner;

    private bool isOn = false;

    private void FixedUpdate()
    {
        var speed = rotateSpeed * Managers.Time.GetFixedDeltaTime(TIME_TYPE.NONE_PLAYER);
        transform.Rotate(new Vector3(speed, speed, speed));
    }
    public void Init(ItemSpawnController spawner)
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
            Managers.Effect.PlayeEffect(Define.EFFECT.EnergeItemEffect,transform);
            Managers.Keyword.AddKeywordMakerGauge(gaugeAmount);
            parentSpawner.RemoveItem(this);
        }
    }

}
