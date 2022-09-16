using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //플레이어를 생성한다.
        PhotonNetwork.Instantiate("Player_Shooter", Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
