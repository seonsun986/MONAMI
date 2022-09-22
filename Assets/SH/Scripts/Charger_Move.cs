using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Charger_Move : MonoBehaviour
{
    //�� ������ �� �� ���ǵ�.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runspeed = 12f;
    public float finalSpeed;
    public float rotSpeed = 5;

    CharacterController cc;
    public Animator anim;
    float animSpeed;


    //�� �� �Ʒ� �� ������ ��ũ�� ���� ��
    public bool run;
    public bool toggleCameraRotation;
    public float smoothness = 10;

    // ī�޶�
    public Camera cam;

    //�������� �ʿ�Ӽ�
    [Header("Jump")]
    public float gravity = -9.81f;
    float yVelocity = 0;
    public float jumpPower = 5;
    //���������� ���� Ȯ��
    bool isJumping = false;

    // �г���
    public TextMeshProUGUI nickName;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    public bool isRun;      //�޸��� Ȯ�ο� ����
    void Update()
    {
        //���� �ѷ����Ⱑ ��Ȱ��ȭ �Ǿ�������
        if (toggleCameraRotation != true)
        {
            //scale : 2���� ���Ͱ��� ������.
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            //slerp : lerp�ʹ� �ٸ��� �����·� ���������̼�����(�������ش�)
            //�÷��̾��� ȸ���� slerp�� Y�� �����̰� �� ��� �������ϰ�
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

        PlayerMove();
    }

    // ���� ���콺�� ��� ������ Aim
    // Aim �� ������ �Ϸ�� ���¿��� ���콺�� ���� �������� ����
    // ���콺 �׳� ������ ������ ������ ���·�
    // �ʿ�Ӽ� : ���� �ð�, �� �ð�
    // ���� �ð��� �귯 �� �ð� �ȿ� ���콺�� �������� ���� �ִϸ��̼� ���
    // �ƴ϶�� Aim �ִϸ��̼� ���
    void PlayerMove()
    {
        // �߷� ���ϱ�
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
            // ���̳� �ڷ� �����϶��� �޸� �� �ִ�
            //isRun = Input.GetKey(KeyCode.LeftShift);
            animSpeed = 1;
            anim.SetLayerWeight(1, 1);

        }
        else
        {
            animSpeed = 0;
            anim.SetLayerWeight(1, 0);

        }

        // ����� �������� ���ظ��� Ȱ��ȭ
        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Aim");
        }
        
        if (Input.GetButtonUp("Fire1"))
        {
            anim.SetTrigger("Fire");
        }

        
        //���� �ٴڿ� ����ִٸ�`
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                anim.Play("Movement");
            }
            //�����ӵ��� 0���� �ϰ�ʹ�.
            yVelocity = 0;
            isJumping = false;
        }
        //������ ���ϰ� ���� �� �׸���!
        //����ڰ� ������ư�� ������ �����ϰ� �ʹ�.
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
