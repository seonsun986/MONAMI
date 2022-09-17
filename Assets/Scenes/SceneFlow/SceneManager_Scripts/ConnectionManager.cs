using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//��Ʈ��ũ�� ����ϱ� ���ؼ� / ����Ÿ���� ����ؾ��� ���� ������ ��κ� Pun�� ����ϸ� �ȴ�.
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
//MonoBehaviourPunCallbacks : 
{
    void Start()
    {
        //PhotonNetwork.GameVersion = "1";
        //�������ڸ��� NameServer ���� (AppID, GameVersion, ����)
        PhotonNetwork.ConnectUsingSettings();
    }
    //������ ������ ���� ����, �κ񸸵�ų� ������ �� ���� ����
    public override void OnConnected()
    {
        base.OnConnected();
        print("������ ������ ���� ����!" + "/ OnConnected");

    }
    //������ ������ ����, �κ� ���� �� ������ ������ ���°� �� ��.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������ ������ ���� ����!" + "/ OnConnectedToMaster");

        //�г��� ����
        PhotonNetwork.NickName = "Player "+ Random.Range(1,100);
        //�⺻ �κ� ���� / ä�ΰ���
        PhotonNetwork.JoinLobby();
        //Ư�� �κ� ���� -- ä�� ������ ���� ��
        //PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("�κ�",Photon.Realtime.LobbyType.Default))
    }

    //�κ� ���� ������ ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� ����!" + "/ OnJoinedLobby");

        //LobbyScene���� �̵�
        PhotonNetwork.LoadLevel("02.Lobby");
    }
    void Update()
    {
        
    }
}
