using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotInProgessLevel : MonoBehaviour
{
    public GameObject deactiveImage;
    public GameObject light2d;

    public void SetUp(bool isUnlock)
    {
        deactiveImage.SetActive(!isUnlock);
        light2d.SetActive(false);
    }
    public void TurnOnLight(bool isTurnOn)
    {
        light2d.SetActive(isTurnOn);
    }
}
