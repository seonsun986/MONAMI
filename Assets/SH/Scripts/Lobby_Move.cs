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
            //scale : 2���� ���Ͱ��� ������.
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            //slerp : lerp�ʹ� �ٸ��� �����·� ���������̼�����(�������ش�)
            //�÷��̾��� ȸ���� slerp�� Y�� �����̰� �� ��� �������ϰ�
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        yVelocity += gravity * Time.deltaTime;
        dir.Normalize();

        // ������ �� �ȱ�
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
