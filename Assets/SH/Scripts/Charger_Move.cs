using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Charger_Move : MonoBehaviourPun
{
    //�� ������ �� �� ���ǵ�.
    [Header("PlayerSpeed")]
    public float speed = 5;
    public float runSpeed = 12f;
    public float finalSpeed;
    public float rotSpeed = 5;
    public float downSpeed = 5;
    public bool isInEnemyInk;

    CharacterController cc;
    public Animator anim;
    public float animSpeed;


    //�� �� �Ʒ� �� ������ ��ũ�� ���� ��
    public bool run;
    public bool toggleCameraRotation;
    public float smoothness = 10;

    // ī�޶�
    public Camera cam;

    //�������� �ʿ�Ӽ�
    [Header("Jump")]
    public float gravity = -9.81f;
    float yVelocity = 0;
    public float jumpPower = 5;
    //���������� ���� Ȯ��
    bool isJumping = false;

    // �г���
    public Text nickName;

    void Start()
    {
        nickName.text = photonView.Owner.NickName;
        DataManager.instance.nickname = photonView.Owner.NickName;
        if (photonView.IsMine == false) return;
        cc = GetComponent<CharacterController>();
    }

    public bool isRun;      //�޸��� Ȯ�ο� ����
    void Update()
    {
        if (photonView.IsMine == false) return;

        if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;

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

    // ���� ���콺�� ��� ������ Aim
    // Aim �� ������ �Ϸ�� ���¿��� ���콺�� ���� �������� ����
    // ���콺 �׳� ������ ������ ������ ���·�
    // �ʿ�Ӽ� : ���� �ð�, �� �ð�
    // ���� �ð��� �귯 �� �ð� �ȿ� ���콺�� �������� ���� �ִϸ��̼� ���
    // �ƴ϶�� Aim �ִϸ��̼� ���
    void PlayerMove()
    {
        // �߷� ���ϱ�
        yVelocity += gravity * Time.deltaTime;
        if (isInEnemyInk == false)
        {
            finalSpeed = (isRun) ? runSpeed : speed;
        }
        else
        {
            finalSpeed = downSpeed;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = h * Vector3.right + v * Vector3.forward;
        yVelocity += gravity * Time.deltaTime;
        dir.Normalize();
        dir = cam.transform.TransformDirection(dir);
       
        if (h != 0 || v != 0)
        {
            // ���̳� �ڷ� �����϶��� �޸� �� �ִ�
            //isRun = Input.GetKey(KeyCode.LeftShift);
            animSpeed = 1;
            photonView.RPC("RPCSetLayerWeight", RpcTarget.All, 1, 1f);

        }
        else
        {
            animSpeed = 0;
            photonView.RPC("RPCSetLayerWeight", RpcTarget.All, 1, 0f);
        }

        // ����� �������� ���ظ��� Ȱ��ȭ
        if(Input.GetButtonDown("Fire1"))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Aim");
        }

        if (Input.GetButtonUp("Fire1"))
        {
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Fire");
        }


        //���� �ٴڿ� ����ִٸ�`
        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement", 0);
                photonView.RPC("RPCAnimPlay", RpcTarget.All, "Movement",1);
            }
            //�����ӵ��� 0���� �ϰ�ʹ�.
            yVelocity = 0;
            isJumping = false;
        }
        //������ ���ϰ� ���� �� �׸���!
        //����ڰ� ������ư�� ������ �����ϰ� �ʹ�.
        if (isJumping == false && Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            photonView.RPC("RPCSetTrigger", RpcTarget.All, "Jump");
            isJumping = true;
        }
        dir.y = yVelocity;
        cc.Move(dir * finalSpeed * Time.deltaTime);
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
    public void RPCAnimPlay(string animPlay, int layer= 0)
    {
        anim.Play(animPlay, layer);
    }

    [PunRPC]
    public void RPCSetLayerWeight(int layerIndex, float weight)
    {
        anim.SetLayerWeight(layerIndex, weight);
    }
}
