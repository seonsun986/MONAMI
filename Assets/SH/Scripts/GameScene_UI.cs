using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameScene_UI : MonoBehaviourPun
{
    public GameObject start_UI;
    public GameObject WhiteFade_UI;
    float currentTime;
    public float offTime = 3;
    public float whiteFade_offTime = 4.5f;

    void Start()
    {
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime>offTime)
        {
            WhiteFade_UI.SetActive(true);
        }
        if(currentTime > whiteFade_offTime)
        {
            start_UI.SetActive(false);
            WhiteFade_UI.SetActive(false);
        }
    }

 

}
