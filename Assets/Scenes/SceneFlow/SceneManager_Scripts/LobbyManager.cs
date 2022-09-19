using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //CreateRoom();
    }
    // Lobby에 2명이 되는 순간 알아서 Room이 만들어지고 그 Room안에 플레이어들을 위치시키고 싶다

    void Update()
    {
        
    }

    //public void JoinLobby2()
    //{
    //    PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("로비2", Photon.Realtime.LobbyType.Default));
    //}

    ////방생성
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    print("로비2 접속 성공!" + "/ OnJoinedLobby");
    //    PhotonNetwork.LoadLevel("03.Lobby2");


    //}


    public void CreateRoom()
    {
        //방 정보 셋팅
        RoomOptions roomOptions = new RoomOptions();

        //최대인원 (0명이면 최대인원, 현재 1:1이 목표이므로 2로 하자)
        roomOptions.MaxPlayers = 2;
        //룸 목록에 보이냐? 보이지 않느냐?  / isVisible을 True하면 보임 기본상태는 트루
        roomOptions.IsVisible = true;

        //방을 만든다
        PhotonNetwork.CreateRoom("XR_A", roomOptions, TypedLobby.Default);
    }
    //방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방생성!!" + "/ OnCreatedRoom");


    }
    //방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방 생성 실패" + ", " + returnCode + ", " + message);

        //팝업.. 똑같은 이름이 있습니다. 다른 이름으로 만드세요

        //방 입장
        JoinRoom();

    }
    //방 입장 요청
    int count;
    public void JoinRoom()
    {
        //PhotonNetwork.LoadLevel("Lobby2");
        PhotonNetwork.JoinRoom("XR_A");
        //PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("로비1", Photon.Realtime.LobbyType.Default));

        //PhotonNetwork.joinRoom                 : 선택한 방에 들어갈 때
        //PhotonNetwork.JoinOrCreateRoom         : 방이름 설정해서 들어가려고 할 때, 해당 이름으로 된 방이 없다면 그 이름으로 방을 생성 후 입장
        //PhotonNetwork.JoinRandomOrCreateRoom   : 랜덤방을 들어가려고 할 때, 조건에 맞는 방이 없다면 내가 방을 생성 후 입장
        //PhotonNetwork.JoinRandomRoom           : 랜덤한 방 들어갈 때
    }

    //방 입장이 성공했을 때 불리는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("03.Lobby2");
    }
    //방 입장이 실패 시 호출되는 함수

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFaild, " + ",  " + returnCode + ", " + message);

    }

    public GameObject popUp;
    public void RegularBtn()
    {
        popUp.SetActive(true);
    }

    public void BtnNo()
    {
        popUp.SetActive(false);
    }


}
