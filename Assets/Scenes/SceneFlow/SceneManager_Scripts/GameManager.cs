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
    public Text[] UI_NickName;
    public GameObject pink_SpawnPoint;
    public GameObject blue_SpawnPoint;

    private void Awake()
    {
        Instance = this;
    }
    int i = 0;

    // ���� �ڵ��� ���� ���̵�� ���� �ӽú���
    int id;
    string weaponName;
    void Start()
    {

        startUI.SetActive(true);
        // ��ŸƮ UI�� �г��� �Ҵ�
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            UI_NickName[i].text = player.NickName;
            i++;
        }


        //int idx = PhotonNetwork.CountOfPlayers - 1;
        //print("���� �÷��̾� �� : " + idx);
        //���� ���� �÷��̾� ���ڿ� ���� �´� �÷��̾ �����Ѵ�.
        // ��ũ(id = 1~3)�� ���(id = 4~6)���� ���� ������ ����Ʈ�� �����ȴ�

        // id�� 1~3�̶�� ��ũ���̴�!
        //if (id >= 1 && id <= 3)
        //{
        //    CreatePlayer("Pink", pink_SpawnPoint.transform.position + pink_SpawnPoint.transform.right * (-10 + (5 * id)));
        //}


        //// id�� 4~6�̶�� ������̴�
        //else if (id >= 4 && id <= 6)
        //{
        //    CreatePlayer("Blue", blue_SpawnPoint.transform.position + blue_SpawnPoint.transform.right * (-10 + (5 * (id - 3))));

        //}

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Charger_Blue", blue_SpawnPoint.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Charger_Pink", pink_SpawnPoint.transform.position, Quaternion.identity);

        }

    }

    public void CreatePlayer(string team, Vector3 spawnPoint)
    {
        // ������ ���Ⱑ �������
        if (weaponName.Contains("Charger"))
        {
            PhotonNetwork.Instantiate("Charger_" + team, spawnPoint, Quaternion.identity);
        }
        
        // ������ ���Ⱑ �ѷ����
        else if (weaponName.Contains("Roller"))
        {
            PhotonNetwork.Instantiate("Roller_" + team, spawnPoint, Quaternion.identity);
        }

        // ������ ���Ⱑ ���Ͷ��
        else if (weaponName.Contains("Shooter"))
        {
            PhotonNetwork.Instantiate("Shooter_" + team, spawnPoint, Quaternion.identity);
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
