using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Result : MonoBehaviourPun
{
    double pink;
    double blue;
    public Text pinkRatio;
    public Text blueRatio;        // 슬라이더의 총 value는 1이다
    public Slider slider;
    public Animator pink_anim;
    public Animator blue_anim;

    //            string seconds = (countTime % 60).ToString("F0");
    void Start()
    {
        // 마스터만 블루 포인트와 레드포인트 다른 사람들에게 넘겨준다

        photonView.RPC("RPCResult", RpcTarget.Others, pink,blue);

        pinkRatio.text = (((pink / (pink + blue)) * 100)).ToString("F1") + "%";
        //pinkRatio.text = string.Format("{0:0.#}", (pink / (pink + blue)) * 100) + "%";
        blueRatio.text = (((blue / (pink + blue)) * 100)).ToString("F1") + "%";
        //blueRatio.text = string.Format("{0:0.#}", (blue / (pink + blue)) * 100) + "%";
        slider.value = (float)(pink / (pink + blue));

        if (pink / (pink + blue) > blue / (pink + blue))
        {
            pink_anim.SetTrigger("Victory");
            blue_anim.SetTrigger("Defeat");
        }
        else
        {
            blue_anim.SetTrigger("Victory");
            pink_anim.SetTrigger("Defeat");
        }
        
    }

    void Update()
    {
        
    }

    [PunRPC]
    public void RPCResult(double pink, double blue)
    {
        pink = (double)DataManager.instance.Pink_point;
        blue = (double)DataManager.instance.Blue_point;
    }
}
