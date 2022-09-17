using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //플레이어를 생성한다.
        PhotonNetwork.Instantiate("TEST_Shooter_Player", Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
