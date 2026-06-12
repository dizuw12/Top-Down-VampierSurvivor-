using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI killCounterText;
    [SerializeField] private TextMeshProUGUI killCounterWinText;
    [SerializeField] private TextMeshProUGUI killCounterLoseText;
    [SerializeField] private float timeLeft = 60f;
    private int zombiekill;
    private bool isGameOver;
    
    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1f;

    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        
        timeLeft -= Time.deltaTime;
        UpdateTimerDisplay();
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            WinGame();
        } 
    }
    private void WinGame()
    {
        isGameOver = true;
        killCounterWinText.text = "Zombies killed: " + zombiekill.ToString();
        Time.timeScale = 0f;
        gameWinPanel.SetActive(true);
    }
    public void GameOver()
    {
        isGameOver = true;
        killCounterLoseText.text = "Zombies killed: " + zombiekill.ToString();
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }
    private void UpdateTimerDisplay()
    {
        
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        if (timeLeft <= 0)
            timerText.text = "DONE!";
        else
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void AddKill()
    {
        zombiekill++;
        killCounterText.text = zombiekill.ToString();

    }
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    
}
