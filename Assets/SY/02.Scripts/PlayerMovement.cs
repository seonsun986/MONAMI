using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator anim;
    Camera cam;
    CharacterController cc;

    //내 지형일 때 런 스피드.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runspeed = 8f;
    public float finalSpeed;
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

    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        cc = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        //마우스 가운데 버튼을 누르면
        if (Input.GetMouseButton(2))
        {//둘러보기 활성화
            toggleCameraRotation = true;
        }
        else
        {//둘러보기 비활성화
            toggleCameraRotation = false;
        }
        //임시로 시프트를 누르면 빠르게 된다고 되어있지만 나중에 색깔을 인식했을 때로 변경 //2개의 조건 달기 나의 색일 때, 오징어로 변했을 때
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else { run = false; }
        PlayerMove();
    }

    //업데이트가 다끝나고 실행되는 LateUpdate
    void LateUpdate()
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
    }
    void PlayerMove()
    {
        //최종 내 스피드는 런이 활성화 되어있으면 빠르게 그게 아니면 보통의 속도로 이동
        finalSpeed = (run) ? runspeed : speed;

        //TransformDirection : 방향을 뜻함
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //right : 지상에서 X좌표를 뜻함
        Vector3 right = transform.transform.TransformDirection(Vector3.right);
        //내가 움직이는 방향은 앞방향 = vertical(세로), 양옆방향 = Horizontal(가로)
        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        //수직 속도 구하기
        yVelocity += gravity * Time.deltaTime;
        //만약 바닥에 닿아있다면
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            //수직속도를 0으로 하고싶다.
            yVelocity = 0;
            isJumping = false;
        }
        //점프를 안하고 있을 때 그리고!
        //사용자가 점프버튼을 누르면 점프하고 싶다.
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            //수직 속도를 변경하고 싶다.
            yVelocity = jumpPower;
            isJumping = true;
        }

        moveDirection.y = yVelocity;

        cc.Move(moveDirection * finalSpeed * Time.deltaTime);

        //애니메이션 블랜더에서 값을 조정 빨라진다면 블랜더 값 1, 걷는다면 0.5f
        //moveDirection.magnitude : 움직일 방향인데 크기만 곱해줌.
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        //블랜더 이름 적고, ,0.1f는 즉각적인 반응, 부드러운 애니메이션 이어지는 효과를 위해서는 값을 높여야함.
        anim.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}
