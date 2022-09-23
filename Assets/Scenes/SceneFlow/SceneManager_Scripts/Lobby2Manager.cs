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
        //��Ʈ��ũ�� ���� ����ȭ �����ֱ�.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // LoadLevel�� �񵿱�� �Ѿ������ ��� 
    bool b;
    void Update()
    {
        if (b == true) return;

        //��Ī Ÿ�̸� ����
        if (matchingCount.gameObject.activeSelf == true)
        {
            countTime += Time.deltaTime;
            string seconds = (countTime % 60).ToString("F0");
            // 10�� �̳��� ��� 0�� �ٿ��ش�
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            matchingCount.text = "��Ī ��.... " + seconds + "��";
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            b = true;
            //CreateRoom();
            //print("�� �������!");
            PhotonNetwork.LoadLevel("SY_Test");

        }
    }

    public void CreateRoom()
    {
        // �� ���� ����
        RoomOptions roomOptions = new RoomOptions();

        //�ִ��ο� (0���̸� �ִ��ο�, ���� 1:1�� ��ǥ�̹Ƿ� 2�� ����)
        roomOptions.MaxPlayers = 2;
        //�� ��Ͽ� ���̳�? ������ �ʴ���?  / isVisible�� True�ϸ� ���� �⺻���´� Ʈ��
        roomOptions.IsVisible = true;

        //���� �����
        PhotonNetwork.CreateRoom("XR_A_1", roomOptions, TypedLobby.Default);
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
        PhotonNetwork.JoinRoom("XR_A_1");
    }

    //�� ������ �������� �� �Ҹ��� �Լ�
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
