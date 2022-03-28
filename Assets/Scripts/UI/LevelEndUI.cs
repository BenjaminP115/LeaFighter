using TMPro;
using UnityEngine;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI xpText;

    void Start()
    {
        killsText.text = "Kills: " + GameManager.Instance.kills;
        xpText.text = "Xp: " + GameManager.Instance.xp;
    }

    void Update()
    {
        
    }
}
