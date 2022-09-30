using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class ShooterMovement : MonoBehaviourPun
{
    Graphics graphics;

    public Animator anim;
    public Camera cam;
    CharacterController cc;

    //�� ������ �� �� ���ǵ�.
    [Header("PlayerSpeed")]
    public float downSpeed = 5;
    public bool isInEnemyInk;
    public float speed = 5;
    public float runSpeed = 12f;
    public float finalSpeed;
    public float rotSpeed = 5;
    public bool isRun;

    public float animSpeed;
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
    public GameObject model;
    Player_HP hp;
    // �г���
    public Text nickName;
    CanHide canHide;

    void Start()
    {
        nickName.text = photonView.Owner.NickName;
        DataManager.instance.nickname = photonView.Owner.NickName;
        if (photonView.IsMine == false) return;
        cc = this.GetComponent<CharacterController>();
        hp = GetComponent<Player_HP>();
        canHide = GetComponent<CanHide>();
    }

    void Update()
    {
        if (photonView.IsMine == false) return;
        if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;
        if (hp.hp <= 0) return;
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

    //������Ʈ�� �ٳ����� ����Ǵ� LateUpdate
    void LateUpdate()
    {
    }
    void PlayerMove()
    {
        //���� �� ���ǵ�� ���� Ȱ��ȭ �Ǿ������� ������ �װ� �ƴϸ� ������ �ӵ��� �̵�
        if (isInEnemyInk == false)
        {
            finalSpeed = (isRun) ? runSpeed : speed;
        }
        else
        {
            finalSpeed = downSpeed;
        }

        //TransformDirection : ������ ����
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //right : ���󿡼� X��ǥ�� ����
        //Vector3 right = transform.transform.TransformDirection(Vector3.right);
        //���� �����̴� ������ �չ��� = vertical(����), �翷���� = Horizontal(����)
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir;

        if (canHide.climbing == false)
        {
            gravity = -9.81f;
            dir = transform.forward * v + transform.right * h;
        }
        else
        {
            dir = transform.up * v + transform.right * h;
        }
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
        if (!canHide.climbing)
        {
            // �߷� ���Ѵ�
            yVelocity += gravity * Time.deltaTime;
        }
        //���� �ٴڿ� ����ִٸ�`
        if (cc.collisionFlags == CollisionFlags.Below && !canHide.climbing)
        {
            //�����ӵ��� 0���� �ϰ�ʹ�.
            if (isJumping)
            {
                photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement", 0);
                photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement", 1);
            }
            yVelocity = 0;
            isJumping = false;
        }
        //������ ���ϰ� ���� �� �׸���!
        //����ڰ� ������ư�� ������ �����ϰ� �ʹ�.
        if (isJumping == false && Input.GetButtonDown("Jump") && !canHide.climbing)
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
            //photonView.RPC("RPCSetTrigger", RpcTarget.All, "Fire");

        }
        if (Input.GetMouseButtonUp(0))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Move");

        }

        if (!canHide.climbing)
        {
            dir.y = yVelocity;
        }
        cc.Move(dir * finalSpeed * Time.deltaTime);


        //�ִϸ��̼� �������� ���� ���� �������ٸ� ���� �� 1, �ȴ´ٸ� 0.5f
        //moveDirection.magnitude : ������ �����ε� ũ�⸸ ������.
        float percent = ((run) ? 1 : 0.5f) * dir.magnitude;
        //���� �̸� ����, ,0.1f�� �ﰢ���� ����, �ε巯�� �ִϸ��̼� �̾����� ȿ���� ���ؼ��� ���� ��������.
        photonView.RPC("RPCSetFloat", RpcTarget.All, animSpeed);
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
    public void RPCAnimPlay(string animPlay, int layer=0)
    {
        anim.Play(animPlay, layer);
    }

    [PunRPC]
    public void RPCCrossFade(string state)
    {
        anim.SetLayerWeight(1, 1);
        anim.CrossFade(state,1,0, 0.7f);
    }
}
