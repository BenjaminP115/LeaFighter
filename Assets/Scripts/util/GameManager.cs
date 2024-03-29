using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int kills;

    private int _xp;
    public int xp
    {
        get { return _xp; }
        set
        {
            _xp = value;

            if (_xp >= levelReq)
            {
                _xp -= levelReq;
                level++;
            }
        }
    }
    public int attrbPoints;

    public int gameLevel = 4;

    public float damage => 10 + damageLevel * 5;
    public float defense => 1 + defenseLevel * 0.1f;
    public float maxHealth => 100 + healthLevel * 10;
    public float health;

    public int damageLevel;
    public int healthLevel;
    public int defenseLevel;

    public int level;
    private int levelReq => 100 + 20 * level;

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

    public void Reset()
    {
        healthLevel = 0;
        damageLevel = 0;
        defenseLevel = 0;

        health = 100;
        kills = 0;
        xp = 0;
        attrbPoints = 0;
        gameLevel = 3;
    }
}