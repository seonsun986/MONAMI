using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_Move : MonoBehaviour
{
    public float speed = 10;
    public float gravity = -9.81f;
    public float jumpPower = 5;
    float yVelocity;
    bool isJumping;
    CharacterController cc;
    public Animator anim;
    float animSpeed;
    bool toggleCameraRotation;
    public GameObject cam;
    public float smoothness = 10;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleCameraRotation != true)
        {
            //scale : 2개의 벡터값을 곱해줌.
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            //slerp : lerp와는 다르게 구형태로 인터폴레이션해줌(보간해준다)
            //플레이어의 회전은 slerp로 Y는 고정이고 좌 우로 스무스하게
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        yVelocity += gravity * Time.deltaTime;
        dir.Normalize();

        // 움직일 때 걷기
        if (h != 0 || v != 0)
        {
            animSpeed = 1;
        }
        else
        {
            animSpeed = 0;
        }

        if (Input.GetButtonDown("Jump") && isJumping == false)
        {
            isJumping = true;
            yVelocity = jumpPower;
            anim.SetTrigger("Jump");
        }
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            anim.Play("Movement");
            yVelocity = 0;
            isJumping = false;
        }
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = yVelocity;
        anim.SetFloat("MoveSpeedAnim", animSpeed);
        cc.Move(dir * speed * Time.deltaTime);
        


    }
}
