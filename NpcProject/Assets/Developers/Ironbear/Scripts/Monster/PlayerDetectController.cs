using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectController : MonoBehaviour
{
    [SerializeField]
    private string detectionTag = "Player";

    [SerializeField]
    private MonsterController monsterController;
    [SerializeField]
    private SphereCollider sphereCollider;
    public float DetectRange { get => sphereCollider.radius; }

    public void SetActive(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(detectionTag))
        {
            Managers.Sound.PlaySFX("Find Monster");
            monsterController.SetStateChase();
        }        
    }
}
