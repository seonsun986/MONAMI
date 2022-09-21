using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoller : MonoBehaviour
{
    //����, ���� �ݶ��̴�
    public GameObject leftRoller;
    public GameObject rightRoller;
    //��ũ ����, ��ũ�� ���� ��ġ = ������ Ÿ�̹��� �ִϸ��̼��̶� ���߱�
    public GameObject inkFactory;
    public GameObject[] inkFirePos;

    //���� �������ΰ�?
    bool isAttack = false;
    void Start()
    {
        leftRoller.SetActive(false);
        rightRoller.SetActive(false);
    }
    void Update()
    {
        //�ѷ� ���� ����
        //��ũ �Ҹ�!
        //���콺 ��ư�� �ѹ� ������ �� ��ũ�� ���� �չ������� ��ũ�� �����ϰ� �߻��Ų��. => �ʿ�Ӽ� : ��ũ����, ��ũ �߻���ġ
        if (Input.GetMouseButtonDown(0))
        {
            RollerInkShoot();
            leftRoller.SetActive(true);
            rightRoller.SetActive(true);
            //������ �����ߴ�!
            print("������ �����ߴ�!");
            isAttack = true;
            //�ִϸ��̼� ���
        }

        //�ѷ��� �����ϴ� ��
        //��ũ �Ҹ�!
        if (Input.GetMouseButton(0))
        {
            print("������ �ϴ����̴�!");
        }

        //�ѷ� ������ ���´�!
        //��ũ �Ҹ� ���� �ʰ� ���ֱ�!
        //�������̿��ٰ� ���콺 ��ư�� ����´°�?
        if (Input.GetMouseButtonUp(0) && isAttack)
        {
            leftRoller.SetActive(false);
            rightRoller.SetActive(false);

            isAttack = false;
            print("������ �����!");
        }

    }
    public void RollerInkShoot()
    {
        for (int i = 0; i < inkFirePos.Length; i++)
        {
            GameObject ink = Instantiate(inkFactory);
            ink.transform.position = inkFirePos[i].transform.position;
            ink.transform.forward = inkFirePos[i].transform.forward;
        }
    }
}
