using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//��Ʈ��ũ�� ����ϱ� ���ؼ� / ����Ÿ���� ����ؾ��� ���� ������ ��κ� Pun�� ����ϸ� �ȴ�.
using Photon.Pun;

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
        print("������ ������ ���� ����!");

    }
    //������ ������ ����, �κ� ���� �� ������ ����
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������ ������ ���� ����!");
    }
    void Update()
    {
        
    }
}
