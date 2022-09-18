using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        int idx = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        print("���� �÷��̾� �� : " + idx);
        //���� ���� �÷��̾� ���ڿ� ���� �´� �÷��̾ �����Ѵ�.
        if (idx == 0)
        {
            PhotonNetwork.Instantiate("TEST_Shooter_Player_Blue", Vector3.zero, Quaternion.identity);
        }
        else if(idx == 1)
        {
            print("��� ĳ���� ����!");
            PhotonNetwork.Instantiate("TEST_Shooter_Player", Vector3.zero, Quaternion.identity);

        }

    }

    void Update()
    {
        
    }
    string instantiateP;
    public List<PhotonView> players = new List<PhotonView>();

    public void CountPlayer(PhotonView pv)
    {
        players.Add(pv);
        // ���࿡ �ο��� 1���̶��
        // Instantiate�ϴ°� �ٲ۴�.
    }
}
