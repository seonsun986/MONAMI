using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ڰ� ���콺 ���� ��ư�� ������
//�⸦ ������(������ ��ƼŬ ���)
//���콺 ��ư�� ���� HitInfo�� �������� �߻�ü�� �߻�����ش�.
//��Ʈ������ ������ �Ÿ��� ���� ��
//�ٴڿ� ���� ������ �ʴ� �ݶ��̴��� �չ������� �Ҹ��� �����ָ� ��ũ�� ������!
public class PlayerCharger : MonoBehaviour
{
    //��Ҵ°�
    bool isAttack = false;

    public GameObject VFX_Charging;

    void Start()
    {
        VFX_Charging.SetActive(false);
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            isAttack = true;
            Charging();
            //�������� ��ø������(minDamage=>maxDamage)
        }
        //����� ���� �÷���
        if(Input.GetMouseButtonUp(0))
        {
            ChargerShot();
            //�������ʴ� �ݶ��̴� transform.pos => hitInfoPos���� �ٴڿ� ����ش�.
            isAttack = false;
            VFX_Charging.SetActive(false);
        }
    }
    void Charging()
    {
        //������� ��ƼŬ ���
        VFX_Charging.SetActive(true);


    }
    void ChargerShot()
    {

    }
}
