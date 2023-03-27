using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class RespawnData
{
    public RespawnData(Transform spot) 
    {
        this.spot = spot;
    }
    public Transform spot;
    public float time;
}


public class ItemSpawnController : MonoBehaviour
{
    [SerializeField]
    private string itemName = "EnergeItem";

    [SerializeField]
    private List<Transform> spawnSpots;
    [SerializeField]
    private float itemRespawnTime = 5f;

    private List<RespawnData> respawns = new List<RespawnData>();
    private void Start()
    {
        for(int i = 0; i < spawnSpots.Count; ++i)
        {
            CreateItem(spawnSpots[i]);
        }
    }
    private void FixedUpdate()
    {

        var lists = respawns.ToList();
        foreach (var item  in lists) 
        {
            item.time += Time.deltaTime;
            if (item.time >= itemRespawnTime)
            {
                CreateItem(item.spot);
                respawns.Remove(item);
            }
        }
    }
    private void CreateItem(Transform spot)
    {
        var item  =  Managers.Resource.Instantiate(itemName, spot).GetComponent<EnergeItemController>();
        item.transform.position = spot.position;
        item.Init(this);
    }
    public void RemoveItem(EnergeItemController item) 
    {
        respawns.Add(new RespawnData(item.transform.parent));
        Managers.Resource.Destroy(item.gameObject);
    }


}
