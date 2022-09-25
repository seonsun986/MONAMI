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
        // 스타트 UI에 닉네임 할당
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            UI_NickName[i].text = player.NickName;
            i++;
        }
        //int idx = PhotonNetwork.CountOfPlayers - 1;
        //print("들어온 플레이어 수 : " + idx);
        //현재 들어온 플레이어 숫자에 따라 맞는 플레이어를 생성한다.
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Charger_Blue", Vector3.zero, Quaternion.identity);
        }
        else
        {
            print("블루 캐릭터 생성!");
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
        // 만약에 인원이 1명이라면
        // Instantiate하는걸 바꾼다.
        if(players.Count >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            countDown.RPC("RpcStartCount", RpcTarget.All);
        }
    }
}
