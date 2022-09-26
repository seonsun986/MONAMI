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

    // 생성 코딩을 위한 아이디와 무기 임시변수
    int id;
    string weaponName;
    void Start()
    {

        startUI.SetActive(true);
        // 스타트 UI에 닉네임 할당
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            UI_NickName[i].text = player.NickName;
            i++;
        }


        //int idx = PhotonNetwork.CountOfPlayers - 1;
        //print("들어온 플레이어 수 : " + idx);
        //현재 들어온 플레이어 숫자에 따라 맞는 플레이어를 생성한다.
        // 핑크(id = 1~3)와 블루(id = 4~6)팀에 따라 리스폰 포인트가 결정된다

        // id가 1~3이라면 핑크팀이다!
        if (DataManager.instance.id >= 1 && DataManager.instance.id <= 3)
        {
            CreatePlayer("Pink", pink_SpawnPoint.transform.position + pink_SpawnPoint.transform.right * (-10 + (5 * DataManager.instance.id)));
        }


        // id가 4~6이라면 블루팀이다
        else if (DataManager.instance.id >=4 && DataManager.instance.id <= 6)
        {
            CreatePlayer("Blue", blue_SpawnPoint.transform.position + blue_SpawnPoint.transform.right * (-10 + (5 * (DataManager.instance.id - 3))));

        }

    }

    public void CreatePlayer(string team, Vector3 spawnPoint)
    {
        // 선택한 무기가 차저라면

        if (DataManager.instance.weaponName.Contains("Charger"))
        {
            PhotonNetwork.Instantiate("Charger_" + team, spawnPoint, Quaternion.identity);
        }
        
        // 선택한 무기가 롤러라면
        else if (DataManager.instance.weaponName.Contains("Roller"))
        {
            PhotonNetwork.Instantiate("Roller_" + team, spawnPoint, Quaternion.identity);
        }

        // 선택한 무기가 슈터라면
        else if (DataManager.instance.weaponName.Contains("Shooter"))
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
        // 만약에 인원이 1명이라면
        // Instantiate하는걸 바꾼다.
        if(players.Count >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            countDown.RPC("RpcStartCount", RpcTarget.All);
        }
    }
}
