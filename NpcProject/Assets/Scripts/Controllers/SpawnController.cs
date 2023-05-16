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
interface ISpawnAble 
{
    public void SetSpawnController(Transform spot , SpawnController controller);


}


public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private string targetItemName = "EnergeItem";

    [SerializeField]
    private List<Transform> spawnSpots;
    [SerializeField]
    private float respawnTime = 5f;

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
        if(respawns.Count <= 0) 
        {
            return;
        }

        var lists = respawns.ToList();
        foreach (var item  in lists) 
        {
            item.time += Time.deltaTime;
            if (item.time >= respawnTime)
            {
                CreateItem(item.spot);
                respawns.Remove(item);
            }
        }
    }
    public virtual void CreateItem(Transform spot)
    {
        var item  =  Managers.Resource.Instantiate(targetItemName, spot).GetComponent<ISpawnAble>();
        item.SetSpawnController(spot, this);
    }
    public virtual void RemoveItem(Transform spot) 
    {
        respawns.Add(new RespawnData(spot));
    }


}
