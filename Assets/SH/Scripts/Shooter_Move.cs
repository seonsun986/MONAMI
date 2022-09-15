using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Move : MonoBehaviour
{
    public float walkSpeed = 10;
    public float runSpeed = 15;
    public float rotSpeed = 5;
    CharacterController cc;
    Animator anim;
    float speed;
    float animSpeed;
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
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        State = state.Idle;
    }

    public bool isRun;      //달리기 확인용 변수
    float h;
    float v;
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        dir.Normalize();

        if (h != 0 || v != 0)
        {
            isRun = false;
            // 앞이나 뒤로 움직일때만 달릴 수 있다
            isRun = Input.GetKey(KeyCode.LeftShift);
            speed = isRun == true ? runSpeed : walkSpeed;
            animSpeed = isRun == true ? 1 : 0.5f;

        }
        else
        {
            speed = 0;
            animSpeed = 0;
        }
        anim.SetFloat("MoveSpeedAnim", animSpeed);

        cc.Move(dir * speed * Time.deltaTime);

        // 다시 회전 돌아가는걸 막기 위한 부분
        if (!(h == 0 && v == 0))
        {
            // 회전하는 부분. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }

        // 점프
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
        }

        // 단발 공격
        if(Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Fire");
        }
        // 연사 공격
        if(Input.GetMouseButton(1))
        {
            anim.CrossFade("Fire", 1, 0, 0.3f);
        }
        
    }
}
