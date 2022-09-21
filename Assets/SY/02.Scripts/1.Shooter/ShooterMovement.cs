using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class ShooterMovement : MonoBehaviourPun, IPunObservable
{
    public Animator anim;
    public Camera cam;
    CharacterController cc;

    //�� ������ �� �� ���ǵ�.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runspeed = 12f;
    public float finalSpeed;
    public float rotSpeed = 5;

    float animSpeed;
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

    // �г���
    public TextMeshProUGUI nickName;
    void Start()
    {
        nickName.text = photonView.Owner.NickName;
        if (photonView.IsMine == false) return;
        cc = this.GetComponent<CharacterController>();

    }

    void Update()
    {

        //�ӽ÷� ����Ʈ�� ������ ������ �ȴٰ� �Ǿ������� ���߿� ������ �ν����� ���� ���� //2���� ���� �ޱ� ���� ���� ��, ��¡��� ������ ��
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else { run = false; }
    }

    //������Ʈ�� �ٳ����� ����Ǵ� LateUpdate
    void LateUpdate()
    {
        if (photonView.IsMine == false) return;

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
    void PlayerMove()
    {
        //���� �� ���ǵ�� ���� Ȱ��ȭ �Ǿ������� ������ �װ� �ƴϸ� ������ �ӵ��� �̵�
        finalSpeed = (run) ? runspeed : speed;

        //TransformDirection : ������ ����
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //right : ���󿡼� X��ǥ�� ����
        //Vector3 right = transform.transform.TransformDirection(Vector3.right);
        //���� �����̴� ������ �չ��� = vertical(����), �翷���� = Horizontal(����)
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = transform.forward * v + transform.right * h;
        dir.Normalize();

        if (v != 0 || h != 0)
        {
            animSpeed = 1;
        }
        else // ����
        {
            animSpeed = 0;
        }

        //���� �ӵ� ���ϱ�
        yVelocity += gravity * Time.deltaTime;
        //���� �ٴڿ� ����ִٸ�`
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            //�����ӵ��� 0���� �ϰ�ʹ�.
            if (isJumping) photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement", 1);
            yVelocity = 0;
            isJumping = false;
        }
        //������ ���ϰ� ���� �� �׸���!
        //����ڰ� ������ư�� ������ �����ϰ� �ʹ�.
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            //���� �ӵ��� �����ϰ� �ʹ�.
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Jump");
            yVelocity = jumpPower;
            isJumping = true;
        }

        // �ܹ� ����
        if (Input.GetMouseButtonDown(0))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Fire");
        }
        // ���� ����
        if (Input.GetMouseButton(0))
        {
            //anim.SetLayerWeight(1, 1);
            //anim.CrossFade("FireForShooter", 1, 0, 0.3f);
            photonView.RPC("RPCCrossFade", RpcTarget.All, "FireForShooter");
        }
        if (Input.GetMouseButtonUp(0))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Move");

        }

        dir.y = yVelocity;
        cc.Move(dir * finalSpeed * Time.deltaTime);


        //�ִϸ��̼� �������� ���� ���� �������ٸ� ���� �� 1, �ȴ´ٸ� 0.5f
        //moveDirection.magnitude : ������ �����ε� ũ�⸸ ������.
        float percent = ((run) ? 1 : 0.5f) * dir.magnitude;
        //���� �̸� ����, ,0.1f�� �ﰢ���� ����, �ε巯�� �ִϸ��̼� �̾����� ȿ���� ���ؼ��� ���� ��������.
        photonView.RPC("RPCSetFloat", RpcTarget.All, animSpeed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    [PunRPC]
    public void RPCSetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    [PunRPC]
    public void RPCSetFloat(float setFloat)
    {
        anim.SetFloat("MoveSpeedAnim", setFloat);
    }

    [PunRPC]
    public void RPCAnimPlay(string animPlay, int layer)
    {
        anim.Play(animPlay, layer);
    }

    [PunRPC]
    public void RPCCrossFade(string state)
    {
        anim.SetLayerWeight(1, 1);
        anim.CrossFade(state, 1, 0, 0.1f);
    }
}
