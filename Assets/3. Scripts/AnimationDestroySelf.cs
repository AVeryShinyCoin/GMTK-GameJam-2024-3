using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroySelf : MonoBehaviour
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
