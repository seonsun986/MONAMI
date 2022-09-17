using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        CreateRoom();
    }

    void Update()
    {

    }

    //�����
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
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("XR_A");
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
        PhotonNetwork.LoadLevel("SY_Test");
    }
    //�� ������ ���� �� ȣ��Ǵ� �Լ�

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFaild, " + ",  " +returnCode + ", " + message);

    }
}
