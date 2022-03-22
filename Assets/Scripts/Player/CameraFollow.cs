using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Min(0f)] private float cameraMoveSpeed = 1f;
    [SerializeField] private Transform target;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, -10f), cameraMoveSpeed * Time.deltaTime);
    }
}
