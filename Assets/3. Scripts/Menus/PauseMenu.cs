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
    [SerializeField] InputAction toggleMenu;
    [SerializeField] InputAction toggleShop;
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
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
        if (winCondition != null)
        {
            winCondition.StartTimer();
        }
    }

    private void Update()
    {
        if (toggleMenu.WasPerformedThisFrame())
        {
            if (GamePaused)
            {
                UntoggleMenus();
                TogglePauseGame(false);
            }
            else
            {
                introScreen.SetActive(true);
                TogglePauseGame(true);
            }
        }
        if (toggleShop.WasPerformedThisFrame())
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
            if (winCondition.QoutaReached)
            {
                scoreText.color = Color.green;
            }
        }
    }

    public void EndGame(int targetScore)
    {
        TogglePauseGame(true);
        GameOver = true;
        if (Score >= targetScore)
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
        toggleMenu.Enable();
        toggleShop.Enable();
        resetGame.Enable();
        toggleMusic.Enable();
        cat.Enable();
    }
    private void OnDisable()
    {
        toggleMenu.Disable();
        toggleShop.Disable();
        resetGame.Disable();
        toggleMusic.Disable();
        cat.Disable();
    }
}
