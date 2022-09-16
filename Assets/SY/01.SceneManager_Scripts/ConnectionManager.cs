using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//네트워크를 사용하기 위해서 / 리얼타임을 사용해야할 일이 있지만 대부분 Pun을 사용하면 된다.
using Photon.Pun;

public class ConnectionManager : MonoBehaviourPunCallbacks
//MonoBehaviourPunCallbacks : 
{
    void Start()
    {
        //PhotonNetwork.GameVersion = "1";
        //시작하자마자 NameServer 접속 (AppID, GameVersion, 지역)
        PhotonNetwork.ConnectUsingSettings();
    }
    //마스터 서버에 접속 성공, 로비만들거나 진입할 수 없는 상태
    public override void OnConnected()
    {
        base.OnConnected();
        print("마스터 서버에 접속 성공!");

    }
    //마스터 서버에 접속, 로비 생성 및 진입이 가능
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 접속 성공!");
    }
    void Update()
    {
        
    }
}
