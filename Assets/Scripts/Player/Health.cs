using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        float normalized = GameManager.Instance.health / GameManager.Instance.maxHealth;
        float a = normalized * 0.25f;
        image.color = Color.HSVToRGB(a, 1, 1);
    }
}
