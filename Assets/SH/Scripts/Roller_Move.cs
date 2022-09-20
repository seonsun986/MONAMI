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

        // 다시 회전 돌아가는걸 막기 위한 부분
        if (!(h == 0 && v == 0))
        {
            // 회전하는 부분. Point 1.
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

        // 공격 상태
        if(Input.GetKeyDown(KeyCode.C))
        {
            // 점프 공격 상태
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                JumpAttack();
            }
            //일반 공격 상태
            else
            {
                anim.SetTrigger("Attack");
            }
            
        }

        // 점프상태
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
        }


        if (State == state.Die)
        {
            UpdateDie();
        }

        // 리깅을 위한 가중치 
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
    // 움직임이 멈추면 다시 Idle 상태로 돌아온다
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
