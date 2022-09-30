using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject orb;

    public Transform firePos;
    public LineRenderer lr;

    public float gravity = -9.81f;
    public float jumpPower = 10;
    float curveDeltaTime = 1 / 60f;

    public int maxPoint = 1000;
    int pointCount = 0;

    //가상의 포지션
    Vector3 pos;

    //마우스 인풋을 받을 변수
    private float rotX;
    //마우스 감도
    public float sensitivity = 100f;

    //던지는 스피드
    public float throw_Speed;

    void Start()
    {
        //처음 시작할 때 각도를 초기화
        rotX = transform.localRotation.eulerAngles.x;
    }

    Vector3 velocity;
    Vector3 firstVelocity;

    void Update()
    {
        //매프레임마다 인풋을 받기 위함
        //X축을 기준으로 카메라가 움직일 때는 마우스를 상하로 움직이니 
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        //rotX : 카메라의 상하 회전의 값을 -70~70으로 한정지어준다
        rotX = Mathf.Clamp(rotX, -50, 50);
        //카메라의 회전을 rot만큼 움직여준다
        Quaternion rot = Quaternion.Euler(rotX, 0, 0);
        transform.rotation = rot;

        //curveDeltaTime = 1 / (float)maxPoint;
        pointCount = 0;
        firstVelocity = velocity = firePos.forward * jumpPower;
        pos = firePos.transform.position;
        lr.positionCount = 2;

        RaycastHit hitInfo;

        if(Input.GetButton("Fire1"))
        {
            //포인트카운트가 맥스 포인트보다 작을 때만 실행.
            for (int i = 0; i < maxPoint; i++)
            {
                if (false == MakeCurve(out hitInfo))
                {
                    //========================================여기서 원하는 것을 실행
                    // 닿은 것이 있다.
                    // 도착지점 파티클 노멀 방향으로 생성.
                    

                    //makeCurve가 false면 for문을 멈춰!
                    break;
                }
            }
        }
        if(Input.GetButtonUp("Fire1"))
        {
            GameObject go = Instantiate(orb);
            go.transform.position = pos;
            go.GetComponent<Rigidbody>().velocity = firstVelocity;
        }

    }
    bool MakeCurve(out RaycastHit hitInfo)
    {

        //첫번째 점일 때(0)는 레이를 쏘지 않겠다. (예외처리)
        if (pointCount == 0)
        {
            lr.positionCount = pointCount + 1;
            lr.SetPosition(pointCount, pos);
            ++pointCount;
            hitInfo = new RaycastHit();
            hitInfo.point = pos;
            return true;
        }

        //중력을 반영
        //gravity : 음수
        velocity += gravity * Vector3.up * curveDeltaTime;
        //가는 방향과 스피드
        pos += velocity * curveDeltaTime;

        //이전 위치에서 새로운 위치로 Ray를 쏘고싶다.
        Vector3 prevPos = lr.GetPosition(pointCount - 1);
        Vector3 dir = pos - prevPos;
        Ray ray = new Ray(prevPos, dir);
        //거리 넣어주기, 레이를 무한히 쏘지말고 점과 점 사이 
        if (Physics.Raycast(ray, out hitInfo, dir.magnitude))
        {
            lr.positionCount = pointCount + 1;
            lr.SetPosition(pointCount, hitInfo.point);
            ++pointCount;
            //부딪힌게 있으면 그만해!!
            return false;
        }
        else
        {
            lr.positionCount = pointCount + 1;
            lr.SetPosition(pointCount, pos);
            ++pointCount;
            //계속해!!
            return true;
        }

    }
}
