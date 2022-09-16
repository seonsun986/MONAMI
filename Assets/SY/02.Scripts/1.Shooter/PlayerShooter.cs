using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//필요속성 : 잉크공장, 발사위치

public class PlayerShooter : MonoBehaviour
{
    //생성위치
    public Transform firePos;
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

    void Start()
    {
    }
    void Update()
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
    private void InkShot()
    {
        Vector3 pos = Camera.main.transform.position;
        pos  =  pos+Camera.main.transform.forward * distance;

        Vector3 dir = (pos - firePos.position).normalized; 

        GameObject ink = Instantiate(InkFactory);
        ink.transform.position = firePos.position;
        ink.transform.forward = dir;
    }
}
