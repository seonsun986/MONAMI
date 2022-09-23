using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class CountDown : MonoBehaviourPun//, IPunObservable
{
    public float countTime = 300;

    public TextMeshProUGUI count;
    public GameObject gameEndImg;


    bool isStart = false;
    void Start()
    {
        
    }

    [PunRPC]
    void RpcStartCount()
    {
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart == false) return;


        if (countTime > 0)
        {
            countTime -= Time.deltaTime;
            string minute = Mathf.FloorToInt(countTime / 60).ToString();
            string seconds = (countTime % 60).ToString("F0");
            // 10초 이내일 경우 0을 붙여준다
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            count.text = minute + " : " + seconds;
            // 60초 짜리는 안띄우기

            if (seconds == "60")
            {
                seconds = "00";
            }
        }

        if (countTime <= 0)
        {
            count.text = "0 : 00";
            Time.timeScale = 0;
            gameEndImg.SetActive(true);
        }

        //if (countTime <= 0)
        //{
        //    count.text = "0 : 00";
        //    Time.timeScale = 0;
        //    gameEndImg.SetActive(true);
        //}

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    if (countTime > 0)
        //    {
        //        countTime -= Time.deltaTime;            
        //    }

        //    if (countTime <= 0)
        //    {
        //        return;
        //    }
        //}

        //if(PhotonNetwork.IsMasterClient)
        //{
        //    if (countTime > 0)
        //    {
        //        string minute = Mathf.FloorToInt(countTime / 60).ToString();
        //        string seconds = (countTime % 60).ToString("F0");
        //        // 10초 이내일 경우 0을 붙여준다
        //        seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

        //        count.text = minute + " : " + seconds;
        //        // 60초 짜리는 안띄우기

        //        if (seconds == "60")
        //        {
        //            seconds = "00";
        //        }
        //    }

        //    if (countTime <= 0)
        //    {
        //        count.text = "0 : 00";
        //        Time.timeScale = 0;
        //        gameEndImg.SetActive(true);
        //    }
        //}

        //else
        //{

        //    if (time > 0)
        //    {
        //        string minute = Mathf.FloorToInt(time / 60).ToString();
        //        string seconds = (time % 60).ToString("F0");
        //        // 10초 이내일 경우 0을 붙여준다
        //        seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

        //        count.text = minute + " : " + seconds;
        //        // 60초 짜리는 안띄우기

        //        if (seconds == "60")
        //        {
        //            seconds = "00";
        //        }
        //    }

        //    if (time <= 0)
        //    {
        //        count.text = "0 : 00";
        //        Time.timeScale = 0;
        //        gameEndImg.SetActive(true);
        //    }
        //}

        // 만약 마스터라면 count 보내기
        // 받아서 coutTime 설정
    }

    //다른 애들이 받기위한 변수
    float time;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(PhotonNetwork.IsMasterClient)
        {        
            stream.SendNext(countTime);
        }

        // 다른 사람이라면 데이터 받기
        else
        {
            time = (float)stream.ReceiveNext();
        }
    }


    
}
