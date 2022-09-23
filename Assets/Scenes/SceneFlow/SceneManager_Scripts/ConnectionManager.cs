using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//��Ʈ��ũ�� ����ϱ� ���ؼ� / ����Ÿ���� ����ؾ��� ���� ������ ��κ� Pun�� ����ϸ� �ȴ�.
using Photon.Pun;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviourPunCallbacks
//MonoBehaviourPunCallbacks : 
{

    // �г��� InputField
    public InputField inputNickname;
    // ���� Button
    public Button btnConnect;

    void Start()
    {
        btnConnect.interactable = false;
        // �г�����(InputField) ����� �� ȣ��Ǵ� �Լ� ���
        inputNickname.onValueChanged.AddListener(OnValueChanged);
        //�г�����(InputField) Focusing�� �Ҿ��� �� ȣ��Ǵ� �Լ� ���
        inputNickname.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnValueChanged(string s)        // ����� ������ ȣ���
    {
        // ���࿡ s�� ���̰� 0���� ũ�ٸ� 
        // ���� ��ư�� Ȱ��ȭ ����
        // �׷��� �ʴٸ� 
        // �ٽ� ���ӹ�ư�� ��Ȱ��ȭ����
        btnConnect.interactable = s.Length > 0;
        print("OnValueChanged : " + s);
    }

    public void OnEndEdit(string s)
    {
        print("OnEditEnd : " + s);
    }
    public void OnClickConnect()
    {
        // NameServer�� ����(APPid,GameVersion, ����)
        PhotonNetwork.ConnectUsingSettings(); // ���� ���� ������ ������ �����ϰڴ�(�ش� �Լ� static)

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
        PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("�κ�1", Photon.Realtime.LobbyType.Default));
        //PhotonNetwork.JoinLobby();
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

    
}
