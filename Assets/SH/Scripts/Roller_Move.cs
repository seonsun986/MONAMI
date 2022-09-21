using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Roller_Move : MonoBehaviour
{
    public float speed = 5;
    public float runSpeed = 10;     // 내 잉크에 있을 때 스피드 값
    public bool run;
    bool isJumping;
    public float finalSpeed;

    public float rotSpeed = 5;
    public float gravity = -9.81f;
    public float jumpPower = 10;
    float yVelocity;
    
    CharacterController cc;
    
    public Animator anim;
    float animSpeed;

    public bool toggleCameraRotation;
    public Camera cam;
    public float smoothness = 10;


    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 만약 둘러보기가 비활성화 되어있으면
        if (toggleCameraRotation != true)
        {
            //scale : 2개의 벡터값을 곱해줌.
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            //slerp : lerp와는 다르게 구형태로 인터폴레이션해줌(보간해준다)
            //플레이어의 회전은 slerp로 Y는 고정이고 좌 우로 스무스하게
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

        PlayerMove();
        //PlayerMove2();



        // 멈춰있을 땐 Idle
        // 움직일 때는 Just Run
        // 마우스를 누르는 순간 일반 공격하고
        // 계속 누르고 움직인다면 롤러를 믿다
        // 점프하고 마우스를 누르면 점프하는 도중에 세로로 잉크를 뿌리는 애니메이션 재생

    }

    void PlayerMove()
    {
        // 중력 더한다
        yVelocity += gravity * Time.deltaTime;
        //최종 내 스피드는 런이 활성화 되어있으면 빠르게 그게 아니면 보통의 속도로 이동
        finalSpeed = (run) ? runSpeed : speed;

        //TransformDirection : 방향을 뜻함
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //right : 지상에서 X좌표를 뜻함
        //Vector3 right = transform.transform.TransformDirection(Vector3.right);
        //내가 움직이는 방향은 앞방향 = vertical(세로), 양옆방향 = Horizontal(가로)
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = transform.forward * v + transform.right * h;
        dir.Normalize();


        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            isJumping = false;
        }

        ////다시 회전 돌아가는걸 막기 위한 부분
        //if (!(h == 0 && v == 0))
        //{
        //    // 회전하는 부분. Point 1.
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        //}

        // 움직일 때
        if (v != 0 || h != 0)
        {
            // 마우스를 누르고 있으면 롤러 내리고 걷기로
            if(Input.GetMouseButton(0))
            {
                animSpeed = 1;
            }    
            else
            {
                animSpeed = 0.5f;
            }
            
        }
        else // 정지
        {
            animSpeed = 0;
        }

        //점프를 안하고 있을 때 그리고!
        //사용자가 점프버튼을 누르면 점프하고 싶다.
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && Input.GetButtonDown("Jump") && isJumping == false)
        {
            anim.SetTrigger("Jump");
            isJumping = true;
            yVelocity = jumpPower;
        }


        // 단발 공격
        if (Input.GetMouseButtonDown(0))
        {
            if(isJumping == true)
            {
                anim.SetTrigger("JumpAttack");
            }
            else
            {
                anim.SetTrigger("Attack");
            }
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            // 롤러 내리고 걷고 있다면
            if(animSpeed == 1)
            {
                animSpeed = 0.5f;
            }
            else
            {
                anim.SetTrigger("Move");
            }
            

        }
        dir.y = yVelocity;
        anim.SetFloat("MovementSpeed", animSpeed);
        cc.Move(dir * finalSpeed * Time.deltaTime);

    }

    

}
