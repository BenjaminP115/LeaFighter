using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private AudioManager audioManager;
    private bool inputBlock;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        if (inputBlock) return;

        inputBlock = true;
        audioManager.Play(0);
        Spawner.enemyAmount = 0;
        PlayerController.isDead = false;
        GameManager.Instance.Reset();
        SceneManager.LoadScene(4);
    }

    public void Credits()
    {
        audioManager.Play(0);
        SceneManager.LoadSceneAsync(2);
    }

    public void Quit()
    {
        audioManager.Play(0);

#if UNITY_STANDALONE
        Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Selected()
    {
        audioManager.Play(1);
    }
}
