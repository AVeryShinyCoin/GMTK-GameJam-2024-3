using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] int levelCap;

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= levelCap + 1)
        {
            Destroy(gameObject);
        }
    }
    public void OnButtonPress()
    {
        SoundManager.Instance.StopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
