using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        //�÷��̾ �����Ѵ�.
        PhotonNetwork.Instantiate("TEST_Shooter_Player", Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
