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

    //현재 공격중인가?
    bool isAttack = false;
    void Start()
    {
        leftRoller.SetActive(false);
        rightRoller.SetActive(false);
    }
    void Update()
    {
        //롤러 공격 시작
        //잉크 소모!
        //마우스 버튼을 한번 눌렀을 때 잉크를 나의 앞방향으로 잉크를 생성하고 발사시킨다. => 필요속성 : 잉크공장, 잉크 발사위치
        if (Input.GetMouseButtonDown(0))
        {
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
        if (Input.GetMouseButton(0))
        {
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
}
