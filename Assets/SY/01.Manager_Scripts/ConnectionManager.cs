using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//네트워크를 사용하기 위해서 / 리얼타임을 사용해야할 일이 있지만 대부분 Pun을 사용하면 된다.
using Photon.Pun;
using Photon.Realtime;

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
        print("마스터 서버에 접속 성공!" + "/ OnConnected");

    }
    //마스터 서버에 접속, 로비 생성 및 진입이 가능한 상태가 된 것.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 접속 성공!" + "/ OnConnectedToMaster");

        //닉네임 설정
        PhotonNetwork.NickName = "Player "+ Random.Range(1,100);
        //기본 로비 진입 / 채널개념
        PhotonNetwork.JoinLobby();
        //특정 로비 진입 -- 채널 나누고 싶을 때
        //PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("로비",Photon.Realtime.LobbyType.Default))
    }

    //로비 접속 성공시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 접속 성공!" + "/ OnJoinedLobby");

        //LobbyScene으로 이동
        PhotonNetwork.LoadLevel("02.Lobby");
    }
    void Update()
    {
        
    }
}
