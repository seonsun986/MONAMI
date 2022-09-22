using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class ShooterMovement : MonoBehaviourPun, IPunObservable
{
    public Animator anim;
    public Camera cam;
    CharacterController cc;

    //내 지형일 때 런 스피드.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runspeed = 12f;
    public float finalSpeed;
    public float rotSpeed = 5;

    float animSpeed;
    //내 발 아래 내 색깔의 잉크가 있을 때
    public bool run;

    public bool toggleCameraRotation;

    public float smoothness = 10;

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
        nickName.text = photonView.Owner.NickName;
        if (photonView.IsMine == false) return;
        cc = this.GetComponent<CharacterController>();

    }

    void Update()
    {

        //임시로 시프트를 누르면 빠르게 된다고 되어있지만 나중에 색깔을 인식했을 때로 변경 //2개의 조건 달기 나의 색일 때, 오징어로 변했을 때
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else { run = false; }
    }

    //업데이트가 다끝나고 실행되는 LateUpdate
    void LateUpdate()
    {
        if (photonView.IsMine == false) return;

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
    void PlayerMove()
    {
        //최종 내 스피드는 런이 활성화 되어있으면 빠르게 그게 아니면 보통의 속도로 이동
        finalSpeed = (run) ? runspeed : speed;

        //TransformDirection : 방향을 뜻함
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //right : 지상에서 X좌표를 뜻함
        //Vector3 right = transform.transform.TransformDirection(Vector3.right);
        //내가 움직이는 방향은 앞방향 = vertical(세로), 양옆방향 = Horizontal(가로)
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = transform.forward * v + transform.right * h;
        dir.Normalize();

        if (v != 0 || h != 0)
        {
            animSpeed = 1;
        }
        else // 정지
        {
            animSpeed = 0;
        }

        //수직 속도 구하기
        yVelocity += gravity * Time.deltaTime;
        //만약 바닥에 닿아있다면`
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            //수직속도를 0으로 하고싶다.
            if (isJumping) photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement", 1);
            yVelocity = 0;
            isJumping = false;
        }
        //점프를 안하고 있을 때 그리고!
        //사용자가 점프버튼을 누르면 점프하고 싶다.
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            //수직 속도를 변경하고 싶다.
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Jump");
            yVelocity = jumpPower;
            isJumping = true;
        }

        // 단발 공격
        if (Input.GetMouseButtonDown(0))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Fire");
        }
        // 연사 공격
        if (Input.GetMouseButton(0))
        {
            //anim.SetLayerWeight(1, 1);
            //anim.CrossFade("FireForShooter", 1, 0, 0.3f);
            photonView.RPC("RPCCrossFade", RpcTarget.All, "FireForShooter");
        }
        if (Input.GetMouseButtonUp(0))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Move");

        }

        dir.y = yVelocity;
        cc.Move(dir * finalSpeed * Time.deltaTime);


        //애니메이션 블랜더에서 값을 조정 빨라진다면 블랜더 값 1, 걷는다면 0.5f
        //moveDirection.magnitude : 움직일 방향인데 크기만 곱해줌.
        float percent = ((run) ? 1 : 0.5f) * dir.magnitude;
        //블랜더 이름 적고, ,0.1f는 즉각적인 반응, 부드러운 애니메이션 이어지는 효과를 위해서는 값을 높여야함.
        photonView.RPC("RPCSetFloat", RpcTarget.All, animSpeed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

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
    public void RPCAnimPlay(string animPlay, int layer)
    {
        anim.Play(animPlay, layer);
    }

    [PunRPC]
    public void RPCCrossFade(string state)
    {
        anim.SetLayerWeight(1, 1);
        anim.CrossFade(state, 1, 0, 0.1f);
    }
}
