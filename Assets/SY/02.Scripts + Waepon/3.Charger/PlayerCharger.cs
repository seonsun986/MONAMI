using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//사용자가 마우스 왼쪽 버튼을 누르면
//기를 모으고(빛나는 파티클 재생)
//마우스 버튼을 떼면 HitInfo의 방향으로 발사체를 발사시켜준다.

public class PlayerCharger : MonoBehaviourPun
{
    public Transform test_cam;
    public Transform test_firePos;

    public Camera cam;
    //쏘았는가
    bool isAttack = false;

    public GameObject VFX_Charging;
    public GameObject chargerInkFactory;
    public GameObject chargerFirePos;


    // 충전 변수
    public int maxInk = 100;
    public int currentInk;
    // 기 모으기 변수(이거에 따라 총알이 나가는 힘과 총알이 줄어드는 개수가 달라진다.)
    // 한번 게이지가 full이면 잉크를 10씩 줄인다.
    // 놓는순간에 currentInk수를 줄인다.
    public int chargeInk = 10;


    public GameObject lazer_pink;
    public GameObject lazer_blue;
    GameObject lazer;
    // crosshair
    public Image crosshair;

    // 쏠 수 있게
    public bool canShoot;
    public bool hideCanShoot;       // 숨었을 때 못쏘게 한다
    public GameObject lowInkUI;

    void Start()
    {
        // GameManager에게 나의 photonView를 주자
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

    // 게이지를 위한 것
    float currentVelocity = 0;

    // 마우스를 누르면 currentFill에 현재 Fillamount를 넣는다
    // chargetime이 지나면 Lerp로 currentFill에서 wantFill로 넣는다
    // 현재시간을 초기화 한다


    // 충전은 여기서!
    // 등에 매는 충전하는 거랑 충전UI랑 count랑 동기화시킨다 // 100이 최대 
    public RectTransform uiInk; // 최대 스케일 : 2.37, 꺼지지 않아있을 때만 스케일 조정한다
    public Transform inkTank;   // 최대 스케일 : 1
    float currentAmount;
    void Update()
    {
        if (!photonView.IsMine) return;
        if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;
        // UI 충전
        // 잉크충전 UI가 켜져있다면
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

        // 잉크탱크 충전
        float inkTankYScale = 0.01f * currentInk;
        inkTank.localScale = new Vector3(inkTank.localScale.x, inkTankYScale, inkTank.localScale.z);


        // 총 쏠 수없는 상태가 되면
        // UI가 켜지긴 해도 충전은 되지 않는다

        if (currentInk <= 0 || hideCanShoot == false)
        {
            /// 잉크부족! UI 띄우기
            if (currentInk <= 0  && lowInkUI.activeSelf == false)
            {
                // 0 보다 작지않게하기
                currentInk = 0;
                lowInkUI.SetActive(true);
            }
           
            canShoot = false;
        }

        else
        {
            // 잉크부족! UI 없애기
            if (lowInkUI.activeSelf == true)
            {
                lowInkUI.SetActive(false);
            }
            canShoot = true;
        }

        // 쏠 수 있을 때만 밑에 것들을 실행한다

        if (canShoot == true)
        {
            if (Input.GetMouseButton(0))
            {
                //Zoom In
                cam.GetComponentInParent<Local_CameraMovement>().zoomDistance = 2f;
                // 차저 레이저관리(로컬)
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

                // 이 UI는 나중에 로컬로 보내야한다
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
                //데미지를 중첩시켜줌(minDamage=>maxDamage) / 리턴 데미지
            }
            //쏘았을 때만 플레이
            if (Input.GetMouseButtonUp(0))
            {
                //Zoom In
                cam.GetComponentInParent<Local_CameraMovement>().zoomDistance = 0f;
                lazer.SetActive(false);
                crosshair.gameObject.SetActive(false);
                crosshair.fillAmount = 0;
                photonView.RPC("RPCChargerShot", RpcTarget.All);
                //보이지않는 콜라이더 transform.pos => hitInfoPos까지 바닥에 깔아준다.
                isAttack = false;
                VFX_Charging.SetActive(false);
                currentInk -= chargeInk;

            }
        }

    }


    RaycastHit Charging()
    {
        //기모으는 파티클 재생
        VFX_Charging.SetActive(true);
        //메인카메라의 위치에서 메인카메라의 앞방향으로 시선을 만들고 싶다.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //print(hitInfo.transform.name);
            //잉크자국을 부딪힌 곳에 남기고싶다.
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

        // 최대 3
        ci.radiusByCharge = currentAmount * 5;

        ink.transform.position = test_firePos.transform.position;
        ink.transform.forward = test_firePos.transform.forward;

        print("총알 잉크 크기" + currentAmount * 3);
    }


    [Header("총알 충전을 위한 변수")]
    float currentTime2;              // 현재 시간
    public float chargerTime = 0.1f;   // 충전 시간
    public int chargeBullet = 5; // 0.1초 마다 충전 개수



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
            // 카운트를 추가 시킨다
            currentInk += chargeBullet;
            currentTime2 = 0;
        }
    }

}

