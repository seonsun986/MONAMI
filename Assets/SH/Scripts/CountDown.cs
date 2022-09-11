using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    public float countTime = 300;
    public TextMeshProUGUI count;

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
            // 10�� �̳��� ��� 0�� �ٿ��ش�
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            count.text = minute + " : " + seconds;
        }
    }
}
