using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int kills;
    public int xp;
    public int attrbPoints;

    public int gameLevel = 3;

    public float damage => 10 + damageLevel * 5;
    public float defense => 1 + defenseLevel * 0.1f;
    public float maxHealth => 100 + healthLevel * 10;
    public float health;

    public int damageLevel;
    public int healthLevel;
    public int defenseLevel;
    public int level => xp / 100;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }
}
