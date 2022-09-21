using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Roller_Move : MonoBehaviour
{
    public float speed = 5;
    public float runSpeed = 10;     // �� ��ũ�� ���� �� ���ǵ� ��
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

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMove();
        //PlayerMove2();



        // �������� �� Idle
        // ������ ���� Just Run
        // ���콺�� ������ ���� �Ϲ� �����ϰ�
        // ��� ������ �����δٸ� �ѷ��� �ϴ�
        // �����ϰ� ���콺�� ������ �����ϴ� ���߿� ���η� ��ũ�� �Ѹ��� �ִϸ��̼� ���

    }

    void PlayerMove()
    {
        // �߷� ���Ѵ�
        yVelocity += gravity * Time.deltaTime;
        //���� �� ���ǵ�� ���� Ȱ��ȭ �Ǿ������� ������ �װ� �ƴϸ� ������ �ӵ��� �̵�
        finalSpeed = (run) ? runSpeed : speed;

        //TransformDirection : ������ ����
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //right : ���󿡼� X��ǥ�� ����
        //Vector3 right = transform.transform.TransformDirection(Vector3.right);
        //���� �����̴� ������ �չ��� = vertical(����), �翷���� = Horizontal(����)
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = transform.forward * v + transform.right * h;
        dir.Normalize();

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            isJumping = false;
        }

        //�ٽ� ȸ�� ���ư��°� ���� ���� �κ�
        if (!(h == 0 && v == 0))
        {
            // ȸ���ϴ� �κ�. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }

        // ������ ��
        if (v != 0 || h != 0)
        {
            // ���콺�� ������ ������ �ѷ� ������ �ȱ��
            if(Input.GetMouseButton(0))
            {
                animSpeed = 1;
            }    
            else
            {
                animSpeed = 0.5f;
            }
            
        }
        else // ����
        {
            animSpeed = 0;
        }

        //������ ���ϰ� ���� �� �׸���!
        //����ڰ� ������ư�� ������ �����ϰ� �ʹ�.
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && Input.GetButtonDown("Jump") && isJumping == false)
        {
            anim.SetTrigger("Jump");
            isJumping = true;
            yVelocity = jumpPower;
        }


        // �ܹ� ����
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
            // �ѷ� ������ �Ȱ� �ִٸ�
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

    void PlayerMove2()
    {
        finalSpeed = (run) ? runSpeed : speed;


        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = transform.forward * v + transform.right * h;
        dir.Normalize();

        //�ٽ� ȸ�� ���ư��°� ���� ���� �κ�
        if (!(h == 0 && v == 0))
        {
            // ȸ���ϴ� �κ�. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }

        if(h==0 && v==0)
        {
            if(Input.GetMouseButtonDown(0))
            {
                anim.Play("Attack");
            }
            else
            {
                anim.Play("Idle");
            }
            
        }
        else
        {
            if(Input.GetMouseButton(0))
            {
                anim.SetTrigger("InkWalk");
            }
            else
            {
                anim.SetTrigger("Walk");
                if (Input.GetMouseButton(0))
                {
                    anim.SetTrigger("InkWalk");
                }
            }
        }

        if(Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                anim.SetTrigger("JumpAttack");
            }
            else
            {
                anim.SetTrigger("Attack");
            }
        }

        cc.Move(dir * finalSpeed * Time.deltaTime);
    }

    

}
