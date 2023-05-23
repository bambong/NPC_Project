using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExLazer : MonoBehaviour
{
    [SerializeField]
    private GameObject RayResult; //충돌하는 위치에 출력할 결과
    [SerializeField]
    private GameObject correctObject;
    [SerializeField]
    private BridgeGameController bridgeGame;

    private bool clear = false;
    public float maxLength = 300f; // 레이저의 최대 길이
    public float laserWidth = 0.1f; // 레이저의 너비

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
    }

    private void Update()
    {
         ShootLaser();
    }

    private void ShootLaser()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, maxLength))
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point);
            RayResult.gameObject.transform.position = hit.point;
            
            // 레이저와 충돌한 물체를 검출하면 실행될 코드 작성
            //Debug.Log(hit.collider.gameObject.name);
            //if(hit.collider.gameObject.name == correctObject.gameObject.name)
            //{
            //    if (clear == false)
            //    {
            //        clear = true;
            //        bridgeGame.clearCount++;
            //        bridgeGame.Clear();
            //    }
            //}
            
            // 충돌한 물체에 대한 추가 처리
            // hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        else
        {
            //if(clear == true)
            //{
            //    clear = false;
            //    bridgeGame.clearCount--;
            //    bridgeGame.Clear();
            //}
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxLength);
            RayResult.gameObject.transform.localPosition = transform.forward * maxLength;
        }
    }
}
