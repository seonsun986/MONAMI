using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator anim;
    Camera cam;
    CharacterController cc;

    //�� ������ �� �� ���ǵ�.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runspeed = 8f;
    public float finalSpeed;
    //�� �� �Ʒ� �� ������ ��ũ�� ���� ��
    public bool run;

    public bool toggleCameraRotation;

    public float smoothness = 10;

    //�������� �ʿ�Ӽ�
    [Header("Jump")]
    public float gravity = -9.81f;
    float yVelocity = 0;
    public float jumpPower = 5;
    //���������� ���� Ȯ��
    bool isJumping = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        cc = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        //���콺 ��� ��ư�� ������
        if (Input.GetMouseButton(2))
        {//�ѷ����� Ȱ��ȭ
            toggleCameraRotation = true;
        }
        else
        {//�ѷ����� ��Ȱ��ȭ
            toggleCameraRotation = false;
        }
        //�ӽ÷� ����Ʈ�� ������ ������ �ȴٰ� �Ǿ������� ���߿� ������ �ν����� ���� ���� //2���� ���� �ޱ� ���� ���� ��, ��¡��� ������ ��
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else { run = false; }
        PlayerMove();
    }

    //������Ʈ�� �ٳ����� ����Ǵ� LateUpdate
    void LateUpdate()
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
    }
    void PlayerMove()
    {
        //���� �� ���ǵ�� ���� Ȱ��ȭ �Ǿ������� ������ �װ� �ƴϸ� ������ �ӵ��� �̵�
        finalSpeed = (run) ? runspeed : speed;

        //TransformDirection : ������ ����
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        //right : ���󿡼� X��ǥ�� ����
        Vector3 right = transform.transform.TransformDirection(Vector3.right);
        //���� �����̴� ������ �չ��� = vertical(����), �翷���� = Horizontal(����)
        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        //���� �ӵ� ���ϱ�
        yVelocity += gravity * Time.deltaTime;
        //���� �ٴڿ� ����ִٸ�
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            //�����ӵ��� 0���� �ϰ�ʹ�.
            yVelocity = 0;
            isJumping = false;
        }
        //������ ���ϰ� ���� �� �׸���!
        //����ڰ� ������ư�� ������ �����ϰ� �ʹ�.
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            //���� �ӵ��� �����ϰ� �ʹ�.
            yVelocity = jumpPower;
            isJumping = true;
        }

        moveDirection.y = yVelocity;

        cc.Move(moveDirection * finalSpeed * Time.deltaTime);

        //�ִϸ��̼� �������� ���� ���� �������ٸ� ���� �� 1, �ȴ´ٸ� 0.5f
        //moveDirection.magnitude : ������ �����ε� ũ�⸸ ������.
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        //���� �̸� ����, ,0.1f�� �ﰢ���� ����, �ε巯�� �ִϸ��̼� �̾����� ȿ���� ���ؼ��� ���� ��������.
        anim.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}
