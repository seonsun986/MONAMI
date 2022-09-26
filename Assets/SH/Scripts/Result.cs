using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
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
        pink = (double)DataManager.instance.Pink_point;
        blue = (double)DataManager.instance.Blue_point;
        pinkRatio.text = (((pink / (pink + blue)) * 100)).ToString("F1") + "%";
        //pinkRatio.text = string.Format("{0:0.#}", (pink / (pink + blue)) * 100) + "%";
        blueRatio.text = (((blue / (pink + blue)) * 100)).ToString("F1") + "%";
        //blueRatio.text = string.Format("{0:0.#}", (blue / (pink + blue)) * 100) + "%";
        slider.value = (float)(pink / (pink + blue));

        if(pink / (pink + blue) > blue / (pink + blue))
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
}
