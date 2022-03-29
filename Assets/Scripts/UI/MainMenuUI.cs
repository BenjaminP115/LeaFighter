using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#else
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
