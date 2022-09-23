using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Lobby2Manager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI matchingCount;
    float countTime;
    void Start()
    {
        //네트워크로 씬을 동기화 시켜주기.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // LoadLevel은 비동기라 넘어갈때까지 계속 
    bool b;
    void Update()
    {
        if (b == true) return;

        //매칭 타이머 관련
        if (matchingCount.gameObject.activeSelf == true)
        {
            countTime += Time.deltaTime;
            string seconds = (countTime % 60).ToString("F0");
            // 10초 이내일 경우 0을 붙여준다
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            matchingCount.text = "매칭 중.... " + seconds + "초";
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            b = true;
            //CreateRoom();
            //print("룸 만들었다!");
            PhotonNetwork.LoadLevel("SY_Test");

        }
    }

    public void CreateRoom()
    {
        // 방 정보 셋팅
        RoomOptions roomOptions = new RoomOptions();

        //최대인원 (0명이면 최대인원, 현재 1:1이 목표이므로 2로 하자)
        roomOptions.MaxPlayers = 2;
        //룸 목록에 보이냐? 보이지 않느냐?  / isVisible을 True하면 보임 기본상태는 트루
        roomOptions.IsVisible = true;

        //방을 만든다
        PhotonNetwork.CreateRoom("XR_A_1", roomOptions, TypedLobby.Default);
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
        PhotonNetwork.JoinRoom("XR_A_1");
    }

    //방 입장이 성공했을 때 불리는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("SY_Test");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFaild, " + ",  " + returnCode + ", " + message);

    }
}
