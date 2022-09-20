using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Roller_Move : MonoBehaviour
{
    public Rig headRig;
    public Rig RarmRig;
    public Rig LarmRig;
    public float speed = 5;
    public float rotSpeed = 5;
    public Animator anim;
    CharacterController cc;
    public enum state
    {
        Idle,
        Move,
        Die
    }
    state State;
    void Start()
    {
        headRig.weight = 1;
        RarmRig.weight = 1;
        LarmRig.weight = 1;
        State = state.Idle;
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    float h;
    float v;
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();
        cc.Move(dir * speed * Time.deltaTime);

        // �ٽ� ȸ�� ���ư��°� ���� ���� �κ�
        if (!(h == 0 && v == 0))
        {
            // ȸ���ϴ� �κ�. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }


        if (State == state.Idle)
        {
            UpdateIdle();
        }
        if (State == state.Move)
        {
            UpdateMove();
        }

        // ���� ����
        if(Input.GetKeyDown(KeyCode.C))
        {
            // ���� ���� ����
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                JumpAttack();
            }
            //�Ϲ� ���� ����
            else
            {
                anim.SetTrigger("Attack");
            }
            
        }

        // ��������
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
        }


        if (State == state.Die)
        {
            UpdateDie();
        }

        // ������ ���� ����ġ 
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            headRig.weight = 0;
            RarmRig.weight = 0;
            LarmRig.weight = 0;
        }
        else
        {
            headRig.weight = 1;
            RarmRig.weight = 1;
            LarmRig.weight = 1;
        }
    }

    private void UpdateIdle()
    {
        if (h != 0 || v!=0)
       {
            State = state.Move;
            anim.SetTrigger("Move");
       }

    }
    // �������� ���߸� �ٽ� Idle ���·� ���ƿ´�
    private void UpdateMove()
    {
        if (h==0 && v==0)
        {
            State = state.Idle;
            anim.SetTrigger("Idle");
        }

    }
    private void UpdateDie()
    {
        
    }

    void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
    }
}
