using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


//필요속성 : 잉크공장, 발사위치

public class PlayerShooter : MonoBehaviourPun
{
    //생성위치
    public GameObject firePos;
    //잉크공장
    public GameObject InkFactory;
    //사거리
    public float distance;
    //잉크 지연 시간 설정
    private float fireRate = 0.1f;
    //다음 잉크 발사시간
    private float nextFire = 0.0f;
    //파티클
    [SerializeField] ParticleSystem inkParticle;

    public Camera cam;

    // 잉크부족 띄우기 위한 것들

    public bool canShoot;
    public GameObject lowInkUI;
    public int count;
    public int maxCount;
    void Start()
    {
        lowInkUI.SetActive(false);
        canShoot = true;
    }

    // 등에 매는 충전하는 거랑 충전UI랑 count랑 동기화시킨다 // 100이 최대 
    public RectTransform uiInk; // 최대 스케일 : 2.37, 꺼지지 않아있을 때만 스케일 조정한다
    public Transform inkTank;   // 최대 스케일 : 1

    void Update()
    {
        // 잉크충전 UI가 켜져있다면
        if(uiInk.gameObject.activeSelf == true)
        {
            if(uiInk.localScale.y > 0)
            {
                float uiYscale = (maxCount - count) * 0.0237f;
                uiInk.localScale = new Vector3(uiInk.localScale.x, uiYscale, uiInk.localScale.z);
            }
            
        }

        float inkTankYScale = 0.01f * (maxCount - count);
        inkTank.localScale = new Vector3(inkTank.localScale.x, inkTankYScale, inkTank.localScale.z);

        // 잉크 탱크 
        // 쏠 수 없게 하기
        if (count > maxCount)
        {
            if (lowInkUI.activeSelf == false)
            {
                lowInkUI.SetActive(true);
            }
            // 넘지 않게하기
            count = maxCount;
            canShoot = false;
        }

        if(canShoot == true)
        {
            //마우스 왼쪽버튼을 누르면
            //Time.time 함수가 nexFire 값보다 클 때만 실행
            if (Input.GetMouseButton(0) && Time.time > nextFire)
            {

                inkParticle.Play();
                //잉크파티클 재생
                nextFire = Time.time + fireRate;
                photonView.RPC("RPCShowBullet", RpcTarget.All);
                //InkShot();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                inkParticle.Stop();

            }
        }
        else    // 쏠 수 없을 때
        {
            if(inkParticle.isPlaying)
            {
                inkParticle.Stop();
            }
            
        }
        
    }
    private void InkShot()
    {
        count++;
        // 카메라 정중앙으로 레이를 쏜다.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
            Vector3 vo = CalculateVelocity(hitInfo.point, firePos.transform.position, 0.2f);
            GameObject ink = Instantiate(InkFactory);
            ink.transform.position = firePos.transform.position;
            ink.transform.forward = firePos.transform.forward;
            ink.GetComponent<Rigidbody>().velocity = vo;
        }
        //Vector3 pos = Camera.main.transform.position;
        //pos  =  pos+Camera.main.transform.forward * distance;

        //Vector3 dir = (pos - firePos.position).normalized; 

        //ink.transform.position = firePos.position;
        //ink.transform.forward = firePos.forward;
    }

    public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        // x, y 길이를 먼저 정한다
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;
        // 길이를 대신하는 float변수
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    // 숨을 때 총알 count 0으로 되살리기 위한 것
    // 쏠 수 없을 때(canShoot == false)
    // 0.1초에 2개씩 총알 충전되도록 한다
    // maxcount보다 count가 많아지면 maxCount로 다시 하게 한다
    // 필요속성 : canShoot, 0.1초마다  충전개수, maxCount, 충전시간, 현재시간

    

    [Header("총알 충전을 위한 변수")]
    float currentTime;              // 현재 시간
    public float chargerTime = 0.1f;   // 충전 시간
    public int chargeBullet = 2; // 0.1초 마다 충전 개수
    public void ChargeInk()
    {
        currentTime += Time.deltaTime;
        if (currentTime > chargerTime)
        {
            if (count <= 0)
            {
                return;
            }
            // 카운트를 추가 시킨다
            count -= chargeBullet;            
            currentTime = 0;
        }
    }
    
    [PunRPC]
    public void RPCShowBullet()
    {
        count++;
        // 카메라 정중앙으로 레이를 쏜다.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 vo = CalculateVelocity(hitInfo.point, firePos.transform.position, 0.2f);
            GameObject ink = Instantiate(InkFactory);
            ink.transform.position = firePos.transform.position;
            ink.transform.forward = firePos.transform.forward;
            ink.GetComponent<Rigidbody>().velocity = vo;
        }
    }
}
