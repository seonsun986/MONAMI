using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;

//���⼭ �뿡 ������ ������� �������ش�.
public class Lobby2Manager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI matchingCount;
    float countTime;

    void Start()
    {
        //��Ʈ��ũ�� ���� ����ȭ �����ֱ�.
        PhotonNetwork.AutomaticallySyncScene = true;
       
    }

    // LoadLevel�� �񵿱�� �Ѿ������ ��� 
    bool b;
    void Update()
    {
        if (b == true) return;

        //��Ī Ÿ�̸� ����
        if (matchingCount.gameObject.activeSelf == true)
        {
            countTime += Time.deltaTime;
            string seconds = (countTime % 60).ToString("F0");
            // 10�� �̳��� ��� 0�� �ٿ��ش�
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            matchingCount.text = "��Ī ��.... " + seconds + "��";
        }
        


        //�뿡 ������ �ο��� ���Ӱ����� �ο��� �Ǹ� ������Ŭ���̾�Ʈ�� ����.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            b = true;
            //CreateRoom();
            //print("�� �������!");
            PhotonNetwork.LoadLevel("SY_Test");

            
        }
        
    }
    

}
