using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoller : MonoBehaviour
{
    //좌측, 우측 콜라이더
    public GameObject leftRoller;
    public GameObject rightRoller;
    //잉크 공장, 잉크의 생성 위치 = 나가는 타이밍을 애니메이션이랑 맞추기
    public GameObject inkFactory;
    public GameObject[] inkFirePos;

    // 잉크 개수 조절
    public int maxInk = 100;
    public int currentInk;

    // 쏠 수 있게
    public bool canShoot;
    public GameObject lowInkUI;

    //현재 공격중인가?
    bool isAttack = false;
    void Start()
    {
        leftRoller.SetActive(false);
        rightRoller.SetActive(false);
        currentInk = maxInk;
    }

    float currentTime;

    // 충전은 여기서!
    // 등에 매는 충전하는 거랑 충전UI랑 count랑 동기화시킨다 // 100이 최대 
    public RectTransform uiInk; // 최대 스케일 : 2.37, 꺼지지 않아있을 때만 스케일 조정한다
    public Transform inkTank;   // 최대 스케일 : 1
    void Update()
    {
        // 잉크충전 UI가 켜져있다면
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

        // 총 쏠 수없는 상태가 되면
        // UI가 켜지긴 해도 충전은 되지 않는다
        float inkTankYScale = 0.01f * currentInk;
        inkTank.localScale = new Vector3(inkTank.localScale.x, inkTankYScale, inkTank.localScale.z);

        // 잉크 탱크
        // 쏠 수 없게 하기
        if (currentInk <=0)
        {
            // 잉크부족! UI 띄우기
            if (lowInkUI.activeSelf == false)
            {
                lowInkUI.SetActive(true);
            }
            // 0보다 작지않게하기
            currentInk = 0;
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

        //롤러 공격 시작
        //잉크 소모!
        //마우스 버튼을 한번 눌렀을 때 잉크를 나의 앞방향으로 잉크를 생성하고 발사시킨다. => 필요속성 : 잉크공장, 잉크 발사위치
        if (Input.GetMouseButtonDown(0) && canShoot == true)
        {
            currentInk -= 4;
            RollerInkShoot();
            leftRoller.SetActive(true);
            rightRoller.SetActive(true);
            //공격을 시작했다!
            print("공격을 시작했다!");
            isAttack = true;
            //애니메이션 재생
        }

        //롤러로 공격하는 중
        //잉크 소모!
        if (Input.GetMouseButton(0) && canShoot == true)
        {
            currentTime += Time.deltaTime;
            if(currentTime>0.2f)
            {
                currentInk -= 3;
                currentTime = 0;
            }
            print("공격을 하는중이다!");
        }

        //롤러 공격을 끝냈다!
        //잉크 소모 하지 않게 해주기!
        //공격중이였다가 마우스 버튼을 때어냈는가?
        if (Input.GetMouseButtonUp(0) && isAttack)
        {
            leftRoller.SetActive(false);
            rightRoller.SetActive(false);

            isAttack = false;
            print("공격을 멈췄다!");
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

    [Header("총알 충전을 위한 변수")]
    float currentTime2;              // 현재 시간
    public float chargerTime = 0.1f;   // 충전 시간
    public int chargeBullet = 10; // 0.1초 마다 충전 개수
    public void ChargeInk()
    {
        currentTime2 += Time.deltaTime;
        if (currentTime2 > chargerTime)
        {
            if (currentInk >= maxInk)
            {
                return;
            }
            // 카운트를 추가 시킨다
            currentInk += chargeBullet;
            currentTime = 0;
        }
    }
}
