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

    // ��ũ ���� ����
    public int maxInk = 100;
    public int currentInk;

    // �� �� �ְ�
    public bool canShoot;
    public GameObject lowInkUI;

    //���� �������ΰ�?
    bool isAttack = false;
    void Start()
    {
        leftRoller.SetActive(false);
        rightRoller.SetActive(false);
        currentInk = maxInk;
    }

    float currentTime;

    // ������ ���⼭!
    // � �Ŵ� �����ϴ� �Ŷ� ����UI�� count�� ����ȭ��Ų�� // 100�� �ִ� 
    public RectTransform uiInk; // �ִ� ������ : 2.37, ������ �ʾ����� ���� ������ �����Ѵ�
    public Transform inkTank;   // �ִ� ������ : 1
    void Update()
    {
        // ��ũ���� UI�� �����ִٸ�
        if (uiInk.gameObject.activeSelf == true)
        {
            if (uiInk.localScale.y >= 0)
            {
                float uiYscale = currentInk * 0.0237f;
                uiInk.localScale = new Vector3(uiInk.localScale.x, uiYscale, uiInk.localScale.z);
            }

            if(uiInk.localScale.y > 2.37f)
            {
                uiInk.localScale = new Vector3(uiInk.localScale.x, 2.37f, uiInk.localScale.z);

            }
        }

        // �� �� ������ ���°� �Ǹ�
        // UI�� ������ �ص� ������ ���� �ʴ´�
        float inkTankYScale = 0.01f * currentInk;
        inkTank.localScale = new Vector3(inkTank.localScale.x, inkTankYScale, inkTank.localScale.z);

        // ��ũ ��ũ
        // �� �� ���� �ϱ�
        if (currentInk <=0)
        {
            // ��ũ����! UI ����
            if (lowInkUI.activeSelf == false)
            {
                lowInkUI.SetActive(true);
            }
            // 0���� �����ʰ��ϱ�
            currentInk = 0;
            canShoot = false;
        }

        else
        {
            // ��ũ����! UI ���ֱ�
            if (lowInkUI.activeSelf == true)
            {
                lowInkUI.SetActive(false);
            }
            canShoot = true;
        }

        //�ѷ� ���� ����
        //��ũ �Ҹ�!
        //���콺 ��ư�� �ѹ� ������ �� ��ũ�� ���� �չ������� ��ũ�� �����ϰ� �߻��Ų��. => �ʿ�Ӽ� : ��ũ����, ��ũ �߻���ġ
        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            currentInk -= 4;
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
        if (Input.GetMouseButton(0) && canShoot == true)
        {
            currentTime += Time.deltaTime;
            if(currentTime>0.2f)
            {
                currentInk -= 3;
                currentTime = 0;
            }
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

    [Header("�Ѿ� ������ ���� ����")]
    float currentTime2;              // ���� �ð�
    public float chargerTime = 0.1f;   // ���� �ð�
    public int chargeBullet = 10; // 0.1�� ���� ���� ����
    public void ChargeInk()
    {
        currentTime2 += Time.deltaTime;
        if (currentTime2 > chargerTime)
        {
            if (currentInk >= maxInk)
            {
                return;
            }
            // ī��Ʈ�� �߰� ��Ų��
            currentInk += chargeBullet;
            currentTime = 0;
        }
    }
}
