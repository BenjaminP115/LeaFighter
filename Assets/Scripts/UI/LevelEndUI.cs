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

    private bool inputBlock;

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
        if (Input.GetKeyUp(KeyCode.Z))
            inputBlock = false;
    }

    public void Select()
    {
        audioManager.Play(1);
    }

    public void HealthUp()
    {
        if (inputBlock) return;

        inputBlock = true;

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
        if (inputBlock) return;

        inputBlock = true;

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
        if (inputBlock) return;

        inputBlock = true;

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
        if (inputBlock) return;

        inputBlock = true;

        if (gm.gameLevel <= 5)
        {
            gm.gameLevel++;
            Spawner.enemyAmount = 0;
            SceneManager.LoadSceneAsync(5);
        }
    }
}
