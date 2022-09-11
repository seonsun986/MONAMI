using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Player의 체력 관리
public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    Slider hpBar;
    public float currentHP;
    public float maxHP = 10f;

    void Start()
    {
        currentHP = maxHP;
        hpBar.value = (float)currentHP / (float)maxHP;
    }

    void Update()
    {
        HP();
    }


    public void OnDamaged(float damage)
    {
        //현재 체력을 맞은 무기의 데미지 값만큼 줄여주고
        currentHP -= damage;
        print("현재 채력 : " + currentHP);
        //만약에 현재 체력이 0보다 같거나 작아지면
        if (currentHP <= 0)
        {
            //나 자신 죽음..
            this.gameObject.SetActive(false);
        }
    }
    void HP()
    {
        hpBar.value = (float)currentHP / (float)maxHP;
    }

}
