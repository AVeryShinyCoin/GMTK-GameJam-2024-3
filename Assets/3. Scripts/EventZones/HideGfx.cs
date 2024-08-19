using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGfx : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
