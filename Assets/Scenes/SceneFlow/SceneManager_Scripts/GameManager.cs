using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        print("들어온 플레이어 수 : " + idx);
        //현재 들어온 플레이어 숫자에 따라 맞는 플레이어를 생성한다.
        if (idx == 0)
        {
            PhotonNetwork.Instantiate("TEST_Shooter_Player_Blue", Vector3.zero, Quaternion.identity);
        }
        else if(idx == 1)
        {
            print("블루 캐릭터 생성!");
            PhotonNetwork.Instantiate("TEST_Shooter_Player", Vector3.zero, Quaternion.identity);

        }

    }

    void Update()
    {
        
    }
    string instantiateP;
    public List<PhotonView> players = new List<PhotonView>();

    public void CountPlayer(PhotonView pv)
    {
        players.Add(pv);
        // 만약에 인원이 1명이라면
        // Instantiate하는걸 바꾼다.
    }
}
