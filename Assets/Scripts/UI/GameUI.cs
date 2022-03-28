using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainText;

    void Start()
    {

    }

    void Update()
    {
        remainText.text = "Remaining Enemies : " + Spawner.enemyAmount;
    }
}
