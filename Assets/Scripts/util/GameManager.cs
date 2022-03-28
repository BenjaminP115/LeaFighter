using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int kills;
    public int xp;

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
