using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] int sceneIndex;

    public void OnButtonPress()
    {
        SoundManager.Instance.StopMusic();
        SceneManager.LoadScene(sceneIndex);
    }
}
