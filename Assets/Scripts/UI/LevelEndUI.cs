using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI defenseText;

    private AudioManager audioManager;
    private GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;
        audioManager = GetComponent<AudioManager>();

        levelText.text = gm.level.ToString();
        pointsText.text = gm.attrbPoints.ToString();
        healthText.text = gm.maxHealth.ToString();
        damageText.text = gm.damage.ToString();
        defenseText.text = gm.defense.ToString();
    }

    void Update()
    {
        
    }

    public void Select()
    {
        audioManager.Play(1);
    }

    public void HealthUp()
    {
        audioManager.Play(0);

        if (gm.attrbPoints > 0)
        {
            gm.healthLevel++;
            healthText.text = gm.maxHealth.ToString();
            gm.attrbPoints--;
            pointsText.text = gm.attrbPoints.ToString();
        }
    }

    public void DamageUp()
    {
        audioManager.Play(0);


        if (gm.attrbPoints > 0)
        {
            gm.damageLevel++;
            damageText.text = gm.damage.ToString();
            gm.attrbPoints--;
            pointsText.text = gm.attrbPoints.ToString();
        }
    }

    public void DefenseUp()
    {
        audioManager.Play(0);

        if (gm.attrbPoints > 0)
        {
            gm.defenseLevel++;
            defenseText.text = gm.defense.ToString();
            gm.attrbPoints--;
            pointsText.text = gm.attrbPoints.ToString();
        }
    }

    public void Exit()
    {
        if (gm.gameLevel <= 5)
        {
            gm.gameLevel++;
            SceneManager.LoadSceneAsync(5);
        }
    }
}
