using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        audioManager.Play(0);
        SceneManager.LoadSceneAsync(1);
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
