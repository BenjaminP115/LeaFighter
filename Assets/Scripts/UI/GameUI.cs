using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI remainingText;
    [SerializeField] private GameObject menu;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        remainingText.text = "Remaining Enemies : " + Spawner.enemyAmount;
    }

    public void Selected()
    {
        audioManager.Play(1);
    }

    public void Continue()
    {
        audioManager.Play(0);
        menu.SetActive(false);
    }

    public void Quit()
    {
        audioManager.Play(0);
        SceneManager.LoadSceneAsync(0);
    }
}
