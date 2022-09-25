using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    public GameObject startUI;
    public Text [] UI_NickName;

    private void Awake()
    {
        Instance = this;
    }
    int i = 0;
    void Start()
    {

        startUI.SetActive(true);
        // ��ŸƮ UI�� �г��� �Ҵ�
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            UI_NickName[i].text = player.NickName;
            i++;
        }
        //int idx = PhotonNetwork.CountOfPlayers - 1;
        //print("���� �÷��̾� �� : " + idx);
        //���� ���� �÷��̾� ���ڿ� ���� �´� �÷��̾ �����Ѵ�.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Charger_Blue", Vector3.zero, Quaternion.identity);
        }
        else
        {
            print("��� ĳ���� ����!");
            PhotonNetwork.Instantiate("Charger_Pink", Vector3.zero, Quaternion.identity);

        }

    }

    void Update()
    {
        
    }
    string instantiateP;
    public List<PhotonView> players = new List<PhotonView>();
    public PhotonView countDown;
    public void CountPlayer(PhotonView pv)
    {
        players.Add(pv);
        // ���࿡ �ο��� 1���̶��
        // Instantiate�ϴ°� �ٲ۴�.
        if(players.Count >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            countDown.RPC("RpcStartCount", RpcTarget.All);
        }
    }
}
