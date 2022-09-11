using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Player�� ü�� ����
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
        //���� ü���� ���� ������ ������ ����ŭ �ٿ��ְ�
        currentHP -= damage;
        print("���� ä�� : " + currentHP);
        //���࿡ ���� ü���� 0���� ���ų� �۾�����
        if (currentHP <= 0)
        {
            //�� �ڽ� ����..
            this.gameObject.SetActive(false);
        }
    }
    void HP()
    {
        hpBar.value = (float)currentHP / (float)maxHP;
    }

}
