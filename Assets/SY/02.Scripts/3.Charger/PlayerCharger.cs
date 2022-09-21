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
    public GameObject VFX_HitImpact;

    //TEST
    public GameObject test_Image;

    void Start()
    {
        VFX_Charging.SetActive(false);
    }

    RaycastHit hitInfo;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isAttack = true;
            hitInfo = Charging();
            //�������� ��ø������(minDamage=>maxDamage)
        }
        //����� ���� �÷���
        if (Input.GetMouseButtonUp(0))
        {
            ChargerShot(hitInfo);
            //�������ʴ� �ݶ��̴� transform.pos => hitInfoPos���� �ٴڿ� ����ش�.
            isAttack = false;
            VFX_Charging.SetActive(false);
        }
    }

    RaycastHit Charging()
    {
        //������� ��ƼŬ ���
        VFX_Charging.SetActive(true);
        //����ī�޶��� ��ġ���� ����ī�޶��� �չ������� �ü��� ����� �ʹ�.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //print(hitInfo.transform.name);
            //��ũ�ڱ��� �ε��� ���� �����ʹ�.
        }
        return hitInfo;
    }
    void ChargerShot(RaycastHit hitInfo)
    {
        GameObject inkImpact = Instantiate(VFX_HitImpact);
        print(hitInfo.transform.name);
        inkImpact.transform.position = hitInfo.point;
        inkImpact.transform.forward = hitInfo.normal;
    }
}

