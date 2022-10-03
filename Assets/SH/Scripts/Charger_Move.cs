using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Charger_Move : MonoBehaviourPun
{
    //내 지형일 때 런 스피드.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runSpeed = 12f;
    public float finalSpeed;
    public float rotSpeed = 5;
    public float downSpeed = 5;
    public bool isInEnemyInk;

    CharacterController cc;
    public Animator anim;
    public float animSpeed;


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
    public Text nickName;
    CanHide canHide;
    OrbGauge orb;
    public GameObject weapon;
    void Start()
    {
        nickName.text = photonView.Owner.NickName;
        DataManager.instance.nickname = photonView.Owner.NickName;
        if (photonView.IsMine == false) return;
        cc = GetComponent<CharacterController>();
        canHide = GetComponent<CanHide>();
        orb = GetComponent<OrbGauge>();
    }

    public bool isRun;      //달리기 확인용 변수
    void Update()
    {
        if (photonView.IsMine == false) return;

        if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;

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
    int count;
    void PlayerMove()
    {
        // 중력 더하기
        if (!canHide.climbing)
        {
            // 중력 더한다
            yVelocity += gravity * Time.deltaTime;
        }

        if (isInEnemyInk == false)
        {
            finalSpeed = (isRun) ? runSpeed : speed;
        }
        else
        {
            finalSpeed = downSpeed;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir;

        if (canHide.climbing == false)
        {
            gravity = -9.81f;
            dir = transform.forward * v + transform.right * h;
        }
        else
        {
            dir = transform.up * v + transform.right * h;
        }
        dir.Normalize();
       
        if (h != 0 || v != 0)
        {
            // 앞이나 뒤로 움직일때만 달릴 수 있다
            //isRun = Input.GetKey(KeyCode.LeftShift);
            animSpeed = 1;
            photonView.RPC("RPCSetLayerWeight", RpcTarget.All, 1, 1f);

        }
        else
        {
            animSpeed = 0;
            photonView.RPC("RPCSetLayerWeight", RpcTarget.All, 1, 0f);
        }

        // R버튼을 누르면 저절로
        // 가 True가 되므로!
        if(orb.isOrb == true && count<1)
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "ThrowAim");
            count++;

        }

        // 누르는 순간에는 조준모드로 활성화
        if (Input.GetButtonDown("Fire1"))
        {
            if(orb.isOrb == true)
            {
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "Throw");
                count = 0;
            }
            else
            {
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "Aim");
            }
        }

        if (Input.GetButtonUp("Fire1") && orb.isOrb == false)
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Fire");
        }


        //만약 바닥에 닿아있다면`
        if (cc.collisionFlags == CollisionFlags.Below && !canHide.climbing)
        {
            if (isJumping)
            {
                photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement", 0);
                photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement",1);
            }
            //수직속도를 0으로 하고싶다.
            yVelocity = 0;
            isJumping = false;
        }
        //점프를 안하고 있을 때 그리고!
        //사용자가 점프버튼을 누르면 점프하고 싶다.
        if (isJumping == false && Input.GetButtonDown("Jump") && !canHide.climbing)
        {
            yVelocity = jumpPower;
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Jump");
            isJumping = true;
        }
        if (!canHide.climbing)
        {
            dir.y = yVelocity;
        }
        cc.Move(dir * finalSpeed * Time.deltaTime);
        photonView.RPC("RPCSetFloat", RpcTarget.All, animSpeed);
    
    }

    [PunRPC]
    public void RPCSetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    [PunRPC]
    public void RPCSetFloat(float setFloat)
    {
        anim.SetFloat("MoveSpeedAnim", setFloat);
    }

    [PunRPC]
    public void RPCAnimPlay(string animPlay, int layer= 0)
    {
        anim.Play(animPlay, layer);
    }

    [PunRPC]
    public void RPCSetLayerWeight(int layerIndex, float weight)
    {
        anim.SetLayerWeight(layerIndex, weight);
    }

}
