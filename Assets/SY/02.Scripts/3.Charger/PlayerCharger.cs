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
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Charging();
            //������� ��ƼŬ ���
            //�������� ��ø������(minDamage=>maxDamage)
        }
        if(Input.GetMouseButtonUp(0))
        {
            ChargerShot();
            //�������ʴ� �ݶ��̴� transform.pos => hitInfoPos���� �ٴڿ� ����ش�.
        }
    }
    void Charging()
    {

    }
    void ChargerShot()
    {

    }
}
