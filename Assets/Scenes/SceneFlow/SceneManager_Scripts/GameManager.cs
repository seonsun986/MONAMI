using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
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
