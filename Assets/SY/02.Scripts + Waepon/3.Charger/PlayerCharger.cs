using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//����ڰ� ���콺 ���� ��ư�� ������
//�⸦ ������(������ ��ƼŬ ���)
//���콺 ��ư�� ���� HitInfo�� �������� �߻�ü�� �߻�����ش�.

public class PlayerCharger : MonoBehaviourPun
{
    public Transform test_cam;
    public Transform test_firePos;

    public Camera cam;
    //��Ҵ°�
    bool isAttack = false;

    public GameObject VFX_Charging;
    public GameObject chargerInkFactory;
    public GameObject chargerFirePos;


    // ���� ����
    public int maxInk = 100;
    public int currentInk;
    // �� ������ ����(�̰ſ� ���� �Ѿ��� ������ ���� �Ѿ��� �پ��� ������ �޶�����.)
    // �ѹ� �������� full�̸� ��ũ�� 10�� ���δ�.
    // ���¼����� currentInk���� ���δ�.
    public int chargeInk = 10;


    public GameObject lazer_pink;
    public GameObject lazer_blue;
    GameObject lazer;
    // crosshair
    public Image crosshair;

    // �� �� �ְ�
    public bool canShoot;
    public bool hideCanShoot;       // ������ �� ����� �Ѵ�
    public GameObject lowInkUI;

    void Start()
    {
        // GameManager���� ���� photonView�� ����
        GameManager.Instance.CountPlayer(photonView);

        VFX_Charging.SetActive(false);
        crosshair.gameObject.SetActive(false);
        crosshair.fillAmount = 0;
        currentInk = maxInk;
        lowInkUI.SetActive(false);
        //lazer.enabled = false;
        if (gameObject.name.Contains("Pink"))
        {
            lazer = Instantiate(lazer_pink);
        }
        if (gameObject.name.Contains("Blue"))
        {
            lazer = Instantiate(lazer_blue);
        }

    }

    RaycastHit hitInfo;

    // �������� ���� ��
    float currentVelocity = 0;

    // ���콺�� ������ currentFill�� ���� Fillamount�� �ִ´�
    // chargetime�� ������ Lerp�� currentFill���� wantFill�� �ִ´�
    // ����ð��� �ʱ�ȭ �Ѵ�


    // ������ ���⼭!
    // � �Ŵ� �����ϴ� �Ŷ� ����UI�� count�� ����ȭ��Ų�� // 100�� �ִ� 
    public RectTransform uiInk; // �ִ� ������ : 2.37, ������ �ʾ����� ���� ������ �����Ѵ�
    public Transform inkTank;   // �ִ� ������ : 1
    float currentAmount;
    void Update()
    {
        if (!photonView.IsMine) return;
        if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;
        // UI ����
        // ��ũ���� UI�� �����ִٸ�
        if (uiInk.gameObject.activeSelf == true)
        {
            if (uiInk.localScale.y >= 0)
            {
                float uiYscale = currentInk * 0.0237f;
                uiInk.localScale = new Vector3(uiInk.localScale.x, uiYscale, uiInk.localScale.z);
            }

            if (uiInk.localScale.y > 2.37f)
            {
                uiInk.localScale = new Vector3(uiInk.localScale.x, 2.37f, uiInk.localScale.z);

            }
        }

        // ��ũ��ũ ����
        float inkTankYScale = 0.01f * currentInk;
        inkTank.localScale = new Vector3(inkTank.localScale.x, inkTankYScale, inkTank.localScale.z);


        // �� �� ������ ���°� �Ǹ�
        // UI�� ������ �ص� ������ ���� �ʴ´�

        if (currentInk <= 0 || hideCanShoot == false)
        {
            /// ��ũ����! UI ����
            if (currentInk <= 0  && lowInkUI.activeSelf == false)
            {
                // 0 ���� �����ʰ��ϱ�
                currentInk = 0;
                lowInkUI.SetActive(true);
            }
           
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

        // �� �� ���� ���� �ؿ� �͵��� �����Ѵ�

        if (canShoot == true)
        {
            if (Input.GetMouseButton(0))
            {
                //Zoom In
                cam.GetComponentInParent<Local_CameraMovement>().zoomDistance = 2f;
                // ���� ����������(����)
                lazer.SetActive(true);
                //lazer.transform.forward = chargerFirePos.transform.forward;
                lazer.GetComponent<LineRenderer>().SetPosition(0, chargerFirePos.transform.position);
                Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    lazer.GetComponent<LineRenderer>().SetPosition(1, hitInfo.point);

                }

                crosshair.gameObject.SetActive(true);

                // �� UI�� ���߿� ���÷� �������Ѵ�
                currentAmount = Mathf.SmoothDamp(crosshair.fillAmount, crosshair.fillAmount + 0.03f, ref currentVelocity, 1f * Time.deltaTime);
                crosshair.fillAmount = currentAmount;
                chargeInk = (int)(currentAmount * 20);
                if (crosshair.fillAmount > 1)
                {
                    chargeInk = 10;
                    crosshair.fillAmount = 1;
                }
                isAttack = true;
                hitInfo = Charging();
                //�������� ��ø������(minDamage=>maxDamage) / ���� ������
            }
            //����� ���� �÷���
            if (Input.GetMouseButtonUp(0))
            {
                //Zoom In
                cam.GetComponentInParent<Local_CameraMovement>().zoomDistance = 0f;
                lazer.SetActive(false);
                crosshair.gameObject.SetActive(false);
                crosshair.fillAmount = 0;
                photonView.RPC("RPCChargerShot", RpcTarget.All);
                //�������ʴ� �ݶ��̴� transform.pos => hitInfoPos���� �ٴڿ� ����ش�.
                isAttack = false;
                VFX_Charging.SetActive(false);
                currentInk -= chargeInk;

            }
        }

    }


    RaycastHit Charging()
    {
        //������� ��ƼŬ ���
        VFX_Charging.SetActive(true);
        //����ī�޶��� ��ġ���� ����ī�޶��� �չ������� �ü��� ����� �ʹ�.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //print(hitInfo.transform.name);
            //��ũ�ڱ��� �ε��� ���� �����ʹ�.
        }
        return hitInfo;
    }

    [PunRPC]
    void RPCChargerShot()
    {
        Debug.DrawRay(test_cam.position, test_cam.forward * 200f, Color.green);
        RaycastHit hithit;

        if (Physics.Raycast(test_cam.position, test_cam.forward, out hithit, 200f))
        {
            test_firePos.LookAt(hithit.point);
            Debug.DrawRay(test_firePos.position, test_firePos.forward * 200f, Color.cyan);
        }

        GameObject ink = Instantiate(chargerInkFactory);
        Charger_Ink ci = ink.GetComponent<Charger_Ink>();

        // �ִ� 3
        ci.radiusByCharge = currentAmount * 5;

        ink.transform.position = test_firePos.transform.position;
        ink.transform.forward = test_firePos.transform.forward;

        print("�Ѿ� ��ũ ũ��" + currentAmount * 3);
    }


    [Header("�Ѿ� ������ ���� ����")]
    float currentTime2;              // ���� �ð�
    public float chargerTime = 0.1f;   // ���� �ð�
    public int chargeBullet = 5; // 0.1�� ���� ���� ����



    public void ChargeInk()
    {
        currentTime2 += Time.deltaTime;
        if (currentTime2 > chargerTime)
        {
            if (currentInk >= maxInk)
            {
                currentInk = maxInk;
                return;
            }
            // ī��Ʈ�� �߰� ��Ų��
            currentInk += chargeBullet;
            currentTime2 = 0;
        }
    }

}

