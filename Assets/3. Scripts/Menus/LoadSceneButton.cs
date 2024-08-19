using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] int sceneIndex;

    public void OnButtonPress()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
