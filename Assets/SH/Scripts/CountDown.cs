using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    public float countTime = 300;
    public TextMeshProUGUI count;
    public GameObject gameEndImg;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countTime > 0)
        {
            countTime -= Time.deltaTime;
            string minute = Mathf.FloorToInt(countTime / 60).ToString();
            string seconds = (countTime % 60).ToString("F0");
            // 10초 이내일 경우 0을 붙여준다
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            count.text = minute + " : " + seconds;
        }

        if(countTime<=0)
        {
            count.text = "0 : 00";
            gameEndImg.SetActive(true);
        }
    }
}
