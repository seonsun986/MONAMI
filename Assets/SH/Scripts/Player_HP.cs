using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_HP : MonoBehaviourPun
{

    public int hp = 1;
    float currentTime;
    public float respawnTime = 3;
    void Start()
    {
        
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        if(hp <=0)
        {
            photonView.RPC("RPCDie", RpcTarget.All);
        }
    }

    public void RPCDie()
    {
        gameObject.SetActive(false);
        currentTime += Time.deltaTime;
        if (currentTime > respawnTime)
        {
            gameObject.SetActive(true);
        }
    }
}
