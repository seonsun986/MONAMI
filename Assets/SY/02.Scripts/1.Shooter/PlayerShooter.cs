using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//필요속성 : 잉크공장, 발사위치

public class PlayerShooter : MonoBehaviour
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
    int count;
    public int maxCount;
    void Start()
    {
        
        canShoot = true;
    }
    void Update()
    {
        if (count > maxCount)
        {
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
                InkShot();
            }
            else if (Input.GetMouseButtonUp(0))
                inkParticle.Stop();
        }
        else
        {
            return;
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
            Vector3 vo = CalculateVelocity(hitInfo.point, firePos.transform.position, 0.5f);
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
}
