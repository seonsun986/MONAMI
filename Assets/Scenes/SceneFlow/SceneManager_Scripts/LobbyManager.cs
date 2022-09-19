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
    // Lobby�� 2���� �Ǵ� ���� �˾Ƽ� Room�� ��������� �� Room�ȿ� �÷��̾���� ��ġ��Ű�� �ʹ�

    void Update()
    {
        
    }

    //public void JoinLobby2()
    //{
    //    PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("�κ�2", Photon.Realtime.LobbyType.Default));
    //}

    ////�����
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    print("�κ�2 ���� ����!" + "/ OnJoinedLobby");
    //    PhotonNetwork.LoadLevel("03.Lobby2");


    //}


    public void CreateRoom()
    {
        //�� ���� ����
        RoomOptions roomOptions = new RoomOptions();

        //�ִ��ο� (0���̸� �ִ��ο�, ���� 1:1�� ��ǥ�̹Ƿ� 2�� ����)
        roomOptions.MaxPlayers = 2;
        //�� ��Ͽ� ���̳�? ������ �ʴ���?  / isVisible�� True�ϸ� ���� �⺻���´� Ʈ��
        roomOptions.IsVisible = true;

        //���� �����
        PhotonNetwork.CreateRoom("XR_A", roomOptions, TypedLobby.Default);
    }
    //�� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�����!!" + "/ OnCreatedRoom");


    }
    //�� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("�� ���� ����" + ", " + returnCode + ", " + message);

        //�˾�.. �Ȱ��� �̸��� �ֽ��ϴ�. �ٸ� �̸����� ���弼��

        //�� ����
        JoinRoom();

    }
    //�� ���� ��û
    int count;
    public void JoinRoom()
    {
        //PhotonNetwork.LoadLevel("Lobby2");
        PhotonNetwork.JoinRoom("XR_A");
        //PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("�κ�1", Photon.Realtime.LobbyType.Default));

        //PhotonNetwork.joinRoom                 : ������ �濡 �� ��
        //PhotonNetwork.JoinOrCreateRoom         : ���̸� �����ؼ� ������ �� ��, �ش� �̸����� �� ���� ���ٸ� �� �̸����� ���� ���� �� ����
        //PhotonNetwork.JoinRandomOrCreateRoom   : �������� ������ �� ��, ���ǿ� �´� ���� ���ٸ� ���� ���� ���� �� ����
        //PhotonNetwork.JoinRandomRoom           : ������ �� �� ��
    }

    //�� ������ �������� �� �Ҹ��� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("03.Lobby2");
    }
    //�� ������ ���� �� ȣ��Ǵ� �Լ�

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
