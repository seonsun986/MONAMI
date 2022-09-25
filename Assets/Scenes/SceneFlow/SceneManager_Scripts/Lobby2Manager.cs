using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;

//여기서 룸에 입장한 사람들을 관리해준다.
public class Lobby2Manager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI matchingCount;
    float countTime;

    void Start()
    {
        //네트워크로 씬을 동기화 시켜주기.
        PhotonNetwork.AutomaticallySyncScene = true;
       
    }

    // LoadLevel은 비동기라 넘어갈때까지 계속 
    bool b;
    void Update()
    {
        if (b == true) return;

        //매칭 타이머 관련
        if (matchingCount.gameObject.activeSelf == true)
        {
            countTime += Time.deltaTime;
            string seconds = (countTime % 60).ToString("F0");
            // 10초 이내일 경우 0을 붙여준다
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            matchingCount.text = "매칭 중.... " + seconds + "초";
        }
        


        //룸에 접속한 인원이 접속가능한 인원이 되면 마스터클라이언트가 시작.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            b = true;
            //CreateRoom();
            //print("룸 만들었다!");
            PhotonNetwork.LoadLevel("SY_Test");

            
        }
        
    }
    

}
