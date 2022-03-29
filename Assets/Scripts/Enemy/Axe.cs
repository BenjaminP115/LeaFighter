using UnityEngine;

public class Axe : MonoBehaviour
{
    public Vector3 moveDir = Vector3.right;

    void Update()
    {
        transform.Translate(moveDir * Time.deltaTime);
    }
}
