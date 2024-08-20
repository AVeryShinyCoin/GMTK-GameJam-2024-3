using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial0 : MonoBehaviour
{
    [SerializeField] GameObject[] tutorialMessages;
    [SerializeField] GameObject[] asteroids;
    [SerializeField] WinCondition winCondition;
    int stage;
    bool tutorialStart;


    public void TutorialStart()
    {
        stage = 0;
        tutorialMessages[0].SetActive(true);
        tutorialMessages[1].SetActive(true);
    }

    private void Update()
    {
        if (asteroids[0] == null && stage == 0)
        {
            stage++;
            Invoke("Stage1", 2f);
        }
        else if (asteroids[1] == null && stage == 1)
        {
            stage++;
            Invoke("Stage2", 2f);
        }
        else if (asteroids[2] == null && stage == 2)
        {
            stage++;
            Invoke("Stage3", 3f);
        }
        else if (stage == 4)
        {
            if (PauseMenu.Instance.Score >= winCondition.TargetScore)
            {
                stage++;
                Invoke("Stage4", 3f);
            }
        }
    }

    void Stage1()
    {
        asteroids[1].SetActive(true);
        tutorialMessages[0].SetActive(false);
        tutorialMessages[1].SetActive(false);
        tutorialMessages[2].SetActive(true);
        tutorialMessages[3].SetActive(true);
        PlayerMain.Instance.EnableSizeChange = true;
    }
    void Stage2()
    {
        asteroids[2].SetActive(true);
        tutorialMessages[2].SetActive(false);
        tutorialMessages[3].SetActive(false);
        tutorialMessages[4].SetActive(true);
    }
    void Stage3()
    {
        stage++;
        PauseMenu.Instance.AddScore(-PauseMenu.Instance.Score);
        FindAnyObjectByType<EnemySpawner>().TurnOffSpawner = false;
        winCondition.gameObject.SetActive(true);
        asteroids[3].SetActive(true);
        asteroids[4].SetActive(true);
        asteroids[5].SetActive(true);
        tutorialMessages[4].SetActive(false);
        tutorialMessages[5].SetActive(true);
    }

    void Stage4()
    {
        PauseMenu.Instance.EndGame(winCondition.TargetScore);

    }
}
