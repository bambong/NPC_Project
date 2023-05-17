using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExLazer : MonoBehaviour
{
    [SerializeField]
    private GameObject RayResult; //충돌하는 위치에 출력할 결과

    public float maxLength = 300f; // 레이저의 최대 길이
    public float laserWidth = 0.1f; // 레이저의 너비
    public Color laserColor = Color.red; // 레이저의 색상

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
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

            // 레이저와 충돌한 물체를 검출하면 실행될 코드 작성
            Debug.Log(hit.collider.gameObject.name);

            // 충돌한 물체에 대한 추가 처리
            // hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * maxLength);
        }
    }
}
