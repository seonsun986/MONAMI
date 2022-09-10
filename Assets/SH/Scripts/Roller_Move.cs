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
    Animator anim;
    CharacterController cc;
    public enum state
    {
        Idle,
        Move,
        Attack,
        Die
    }
    state State;
    void Start()
    {
        headRig.weight = 1;
        RarmRig.weight = 1;
        LarmRig.weight = 1;
        State = state.Idle;
        anim = GetComponent<Animator>();
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
        if (State == state.Idle)
        {
            UpdateIdle();
        }
        if (State == state.Move)
        {
            UpdateMove();
        }
        if (State == state.Attack)
        {
            UpdateAttack();

        }
        if (State == state.Die)
        {
            UpdateDie();
        }
    }

    private void UpdateIdle()
    {
       if(h != 0 || v!=0)
       {
            State = state.Move;
            anim.SetTrigger("Move");
       }
    }
    // 움직임이 멈추면 다시 Idle 상태로 돌아온다
    private void UpdateMove()
    {
        if(h==0 && v==0)
        {
            State = state.Idle;
            anim.SetTrigger("Idle");
        }
    }

    private void UpdateAttack()
    {
        
    }

    private void UpdateDie()
    {
        
    }
}
