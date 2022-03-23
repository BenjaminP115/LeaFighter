using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField, Min(0)] private float health;
    private float maxHealth;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        maxHealth = health;
    }

    void Update()
    {
        float normalized = health / maxHealth;
        float a = normalized * 0.25f;
        image.color = Color.HSVToRGB(a, 1, 1);
    }
}
