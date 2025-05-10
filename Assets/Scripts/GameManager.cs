using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;

    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(uiCanvas);
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);
    }

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;
    
        if(camera != null)
        {
            camera.cullingMask = 0;
        }
        UpdateHUD();
        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    public void LevelComplete()
    {
        score += 1000;

        int nextLevel = level + 1;
        
        if(nextLevel >= SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 1;
        }
        LoadLevel(nextLevel);
    }

    public void LevelFailed()
    {
        lives--;

        if(lives < 0)
        {
            NewGame();
        }
        else
        {
            LoadLevel(level);
        }
    }

    private void UpdateHUD()
    {
        livesText.text = lives.ToString();
        scoreText.text = score.ToString();
    }
}
