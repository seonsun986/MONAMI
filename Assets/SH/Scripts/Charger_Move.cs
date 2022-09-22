using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Charger_Move : MonoBehaviour
{
    //내 지형일 때 런 스피드.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runspeed = 12f;
    public float finalSpeed;
    public float rotSpeed = 5;

    CharacterController cc;
    public Animator anim;
    float animSpeed;


    //내 발 아래 내 색깔의 잉크가 있을 때
    public bool run;
    public bool toggleCameraRotation;
    public float smoothness = 10;

    // 카메라
    public Camera cam;

    //점프관련 필요속성
    [Header("Jump")]
    public float gravity = -9.81f;
    float yVelocity = 0;
    public float jumpPower = 5;
    //점프중인지 여부 확인
    bool isJumping = false;

    // 닉네임
    public TextMeshProUGUI nickName;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    public bool isRun;      //달리기 확인용 변수
    void Update()
    {
        //만약 둘러보기가 비활성화 되어있으면
        if (toggleCameraRotation != true)
        {
            //scale : 2개의 벡터값을 곱해줌.
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            //slerp : lerp와는 다르게 구형태로 인터폴레이션해줌(보간해준다)
            //플레이어의 회전은 slerp로 Y는 고정이고 좌 우로 스무스하게
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

        PlayerMove();
    }

    // 왼쪽 마우스를 계속 누르면 Aim
    // Aim 후 차지가 완료된 상태에서 마우스를 떼면 공격으로 간다
    // 마우스 그냥 왼쪽을 눌러도 공격인 상태로
    // 필요속성 : 현재 시간, 뗀 시간
    // 현재 시간이 흘러 뗀 시간 안에 마우스를 떼버리면 공격 애니메이션 재생
    // 아니라면 Aim 애니메이션 재생
    void PlayerMove()
    {
        // 중력 더하기
        yVelocity += gravity * Time.deltaTime;
        finalSpeed = isRun == true ? runspeed : speed;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        yVelocity += gravity * Time.deltaTime;
        dir.Normalize();
        dir = transform.TransformDirection(dir);
       
        if (h != 0 || v != 0)
        {
            // 앞이나 뒤로 움직일때만 달릴 수 있다
            //isRun = Input.GetKey(KeyCode.LeftShift);
            animSpeed = 1;
            anim.SetLayerWeight(1, 1);

        }
        else
        {
            animSpeed = 0;
            anim.SetLayerWeight(1, 0);

        }

        // 누루는 순간에는 조준모드로 활성화
        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Aim");
        }
        
        if (Input.GetButtonUp("Fire1"))
        {
            anim.SetTrigger("Fire");
        }

        
        //만약 바닥에 닿아있다면`
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                anim.Play("Movement");
            }
            //수직속도를 0으로 하고싶다.
            yVelocity = 0;
            isJumping = false;
        }
        //점프를 안하고 있을 때 그리고!
        //사용자가 점프버튼을 누르면 점프하고 싶다.
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            anim.SetTrigger("Jump");
            isJumping = true;
        }

        anim.SetFloat("MoveSpeedAnim", animSpeed);
        dir.y = yVelocity;
        cc.Move(dir * finalSpeed * Time.deltaTime);
    }
}
