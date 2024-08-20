using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavButton : MonoBehaviour
{
    [SerializeField] List<GameObject> revealedObject = new();
    [SerializeField] List<GameObject> hiddenObjects = new();

    public void OnButtonPress()
    {
        foreach(GameObject gob in hiddenObjects)
        {
            gob.SetActive(false);
        }
        foreach(GameObject gob in revealedObject)
        {
            gob.SetActive(true);
        }
    }
}
