using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamStart : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] sr;

    [SerializeField] Sprite greenBeam;
    [SerializeField] Sprite orangeBeam;
    [SerializeField] Sprite purpleBeam;


    public void OrangeBeam()
    {
        sr[0].sprite = orangeBeam;
        sr[1].sprite = orangeBeam;
        gameObject.SetActive(true);
    }
    public void GreenBeam()
    {
        sr[0].sprite = greenBeam;
        sr[1].sprite = greenBeam;
        gameObject.SetActive(true);
    }
    public void PurpleBeam()
    {
        sr[0].sprite = purpleBeam;
        sr[1].sprite = purpleBeam;
        gameObject.SetActive(true);
    }

    public void TurnOffBeam()
    {
        gameObject.SetActive(false);
    }
}
