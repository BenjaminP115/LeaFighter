using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, -10f), 1 * Time.deltaTime);
    }
}
