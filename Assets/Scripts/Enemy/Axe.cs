using UnityEngine;

public class Axe : MonoBehaviour
{
    public Vector3 facingDirection = Vector3.right;

    void Update()
    {
        transform.Translate(facingDirection.normalized * Time.deltaTime);
    }
}
