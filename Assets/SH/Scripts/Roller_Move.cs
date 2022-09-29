using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;


public class Roller_Move : MonoBehaviourPun
{
    public float speed = 5;
    public float runSpeed = 13;     // �� ��ũ�� ���� �� ���ǵ� ��
    public float downSpeed = 5;
    public bool isInEnemyInk;
    public bool isRun;
    public bool isJumping;
    public float finalSpeed;

    public float rotSpeed = 5;
    public float gravity = -9.81f;
    public float jumpPower = 10;
    float yVelocity;
    
    CharacterController cc;
    
    public Animator anim;
    public float animSpeed;

    public bool toggleCameraRotation;
    public Camera cam;
    public float smoothness = 10;

    // �г���
    public Text nickName;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        nickName.text = photonView.Owner.NickName;
        DataManager.instance.nickname = photonView.Owner.NickName;

    }

    void Update()
    {
        if (photonView.IsMine == false) return;
        if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;


        // ���� �ѷ����Ⱑ ��Ȱ��ȭ �Ǿ�������
        if (toggleCameraRotation != true)
        {
            //scale : 2���� ���Ͱ��� ������.
            Vector3 playerRotate = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            //slerp : lerp�ʹ� �ٸ��� �����·� ���������̼�����(�������ش�)
            //�÷��̾��� ȸ���� slerp�� Y�� �����̰� �� ��� �������ϰ�
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }

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
        if(isInEnemyInk == false)
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
        Vector3 dir = transform.forward * v + transform.right * h;
        dir.Normalize();


        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            isJumping = false;
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
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Jump");
            isJumping = true;
            yVelocity = jumpPower;
        }


        // �ܹ� ����
        if (Input.GetMouseButtonDown(0))
        {
            if(isJumping == true)
            {
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "JumpAttack");
            }
            else
            {
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "Attack");
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
                photonView.RPC("RPCSetTrigger", RpcTarget.All, "Move");
            }
            

        }
        dir.y = yVelocity;
        photonView.RPC("RPCSetFloat", RpcTarget.All, animSpeed);
        cc.Move(dir * finalSpeed * Time.deltaTime);

    }

    [PunRPC]
    public void RPCSetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    [PunRPC]
    public void RPCSetFloat(float setFloat)
    {
        anim.SetFloat("MovementSpeed", setFloat);
    }

    [PunRPC]
    public void RPCAnimPlay(string animPlay, int layer)
    {
        anim.Play(animPlay, layer);
    }

}
