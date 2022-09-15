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

    public bool isRun;      //�޸��� Ȯ�ο� ����
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
            // ���̳� �ڷ� �����϶��� �޸� �� �ִ�
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

        // �ٽ� ȸ�� ���ư��°� ���� ���� �κ�
        if (!(h == 0 && v == 0))
        {
            // ȸ���ϴ� �κ�. Point 1.
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }

        // ����
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jump");
        }

        // �ܹ� ����
        if(Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Fire");
        }
        // ���� ����
        if(Input.GetMouseButton(1))
        {
            anim.CrossFade("Fire", 1, 0, 0.3f);
        }
        
    }
}
