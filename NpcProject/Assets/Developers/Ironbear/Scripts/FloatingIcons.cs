using UnityEngine;

public class FloatingIcons : MonoBehaviour
{
    private float floatingSpeed = 5f;

    private void Awake()
    {
        floatingSpeed = CreditPanelController.animSpeed;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * floatingSpeed * 3f * Time.deltaTime);
    }
}
