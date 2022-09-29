using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;


public class Roller_Move : MonoBehaviourPun
{
    public float speed = 5;
    public float runSpeed = 13;     // 내 잉크에 있을 때 스피드 값
    public float downSpeed = 5;
    public bool isInEnemyInk;
    public bool isRun;
    public bool isJumping;
    public float finalSpeed;

    public float rotSpeed = 5;
    public float gravity = -9.81f;
    public float jumpPower = 10;
    float yVelocity;
    
    CharacterController cc;
    
    public Animator anim;
    public float animSpeed;

    public bool toggleCameraRotation;
    public Camera cam;
    public float smoothness = 10;

    // 닉네임
    public Text nickName;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        nickName.text = photonView.Owner.NickName;
        DataManager.instance.nickname = photonView.Owner.NickName;

    }

    void Update()
    {
        if (photonView.IsMine == false) return;
        if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;


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
        if(isInEnemyInk == false)
        {
            finalSpeed = (isRun) ? runSpeed : speed;
        }
        else
        {
            finalSpeed = downSpeed;
        }
        

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
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Jump");
            isJumping = true;
            yVelocity = jumpPower;
        }


        // 단발 공격
        if (Input.GetMouseButtonDown(0))
        {
            if(isJumping == true)
            {
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "JumpAttack");
            }
            else
            {
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "Attack");
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
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "Move");
            }
            

        }
        dir.y = yVelocity;
        photonView.RPC("RPCSetFloat", RpcTarget.All, animSpeed);
        cc.Move(dir * finalSpeed * Time.deltaTime);

    }

    [PunRPC]
    public void RPCSetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    [PunRPC]
    public void RPCSetFloat(float setFloat)
    {
        anim.SetFloat("MovementSpeed", setFloat);
    }

    [PunRPC]
    public void RPCAnimPlay(string animPlay, int layer)
    {
        anim.Play(animPlay, layer);
    }

}
