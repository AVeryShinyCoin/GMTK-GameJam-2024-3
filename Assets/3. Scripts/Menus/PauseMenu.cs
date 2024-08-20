using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    [SerializeField] bool testMode;
    public int Score;
    public bool GameOver;

    [Space(20)]
    [SerializeField] InputAction togglePause;
    [SerializeField] InputAction resetGame;
    [SerializeField] InputAction toggleMusic;
    [SerializeField] InputAction cat;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject introScreen;
    [SerializeField] GameObject shopMenu;

    [SerializeField] GameOverScreen gameOverScreen;
    [SerializeField] WinCondition winCondition;

    public bool GamePaused;
    bool musicPlaying = true;
    bool firstUnpause = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (!testMode)
        {
            introScreen.SetActive(true);
            TogglePauseGame(true);
        }
        AddScore(0);
        if (winCondition != null
            && SceneManager.GetActiveScene().buildIndex != 1)
        {
            winCondition.StartTimer();
        }
    }

    private void Update()
    {
        if (togglePause.WasPerformedThisFrame())
        {
            if (GamePaused)
            {
                UntoggleMenus();
                TogglePauseGame(false);
            }
            else
            {
                shopMenu.SetActive(true);
                TogglePauseGame(true);
            }

            if (firstUnpause)
            {
                firstUnpause = false;
                Tutorial0 tutorial = FindAnyObjectByType<Tutorial0>();
                if (tutorial != null) tutorial.TutorialStart();
            }
        }

        if (resetGame.WasPerformedThisFrame())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        if (toggleMusic.WasPerformedThisFrame())
        {
            musicPlaying = !musicPlaying;
            FindAnyObjectByType<SoundVolumeSetting>().ToggleMusic(musicPlaying);
        }

        if (cat.WasPerformedThisFrame())
        {
            Debug.Log("triggered");
            SoundManager.Instance.PlaySound("Cat");
        }
    }

    void UntoggleMenus()
    {
        introScreen.SetActive(false);
        shopMenu.SetActive(false);
    }

    void TogglePauseGame(bool toggle)
    {
        if (GameOver) return;
        GamePaused = toggle;

        if (GamePaused)
        {
            Time.timeScale = 0;
            SoundManager.Instance.StopSound("ShrinkBeamLoop");
            SoundManager.Instance.StopSound("EnlargeBeamLoop");
            SoundManager.Instance.StopSound("TractorBeamLoop");
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void AddScore(int value)
    {
        Score += value;
        scoreText.text = "$" + Score;
        if (winCondition != null)
        {
            winCondition.UpdateSlider(Score);
            if (winCondition.QuotaReached)
            {
                scoreText.color = Color.green;
            }
        }
    }

    public void InvokeGameOver()
    {
        Invoke("EndGamePlayerDeath", 2f);
        GameOver = true;
    }
    public void EndGamePlayerDeath()
    {
        EndGame(winCondition.TargetScore);
    }
    public void EndGame(int targetScore)
    {
        Time.timeScale = 0;
        GameOver = true;
        GamePaused = true;
        SoundManager.Instance.StopSound("ShrinkBeamLoop");
        SoundManager.Instance.StopSound("EnlargeBeamLoop");
        SoundManager.Instance.StopSound("TractorBeamLoop");
        if (Score >= targetScore || winCondition.InfiniteMode)
        {
            gameOverScreen.DisplayVictory(Score, targetScore, 0);
        }
        else
        {
            gameOverScreen.DisplayFailure(Score, targetScore, 0);
        }
    }


    private void OnEnable()
    {
        togglePause.Enable();
        resetGame.Enable();
        toggleMusic.Enable();
        cat.Enable();
    }
    private void OnDisable()
    {
        togglePause.Disable();
        resetGame.Disable();
        toggleMusic.Disable();
        cat.Disable();
    }
}
