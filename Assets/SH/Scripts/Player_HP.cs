using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player_HP : MonoBehaviourPun
{
    // ȭ�� �ǰݽ� ��ũ�� ��Ƽ����
    public Material screenMaterial;

    Transform pink_RespawnPoint;
    Transform blue_RespawnPoint;

    public int hp = 1;
    float currentTime;
    public float respawnTime = 3;

    // ī�޶�� ����־���Ѵ�
    public GameObject body;
    public GameObject weapon;
    public GameObject inkTank;

    // �÷��̾��� ���̵� �־��ֱ� ���� ����
    public string weaponName;

    // (�����̸�)���� ���ߴ�! �ִ� ���� ������Ʈ
    public GameObject killMsgBox;
    // (�����̸�)���� ���ߴ�! �־��� �ؽ�Ʈ
    public Text killMsgtxt;
    bool isRepawned;
    float currentTime2;

    void Start()
    {
        // Ǯ ��ũ�� ��������
        screenMaterial = Resources.Load<Material>("Voronoi_Fykk/screen_tut");

        pink_RespawnPoint = GameObject.Find("PinkTeam_Respawn").transform;
        blue_RespawnPoint = GameObject.Find("BlueTeam_Respawn").transform;
        killMsgBox.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            screenMaterial.SetFloat("_FullscreenIntensity", 0.5f);
        }

        if (!photonView.IsMine) return;
        if (hp <=0)
        {
            // ������ �ɶ����� ���� ���� �˷��ִ� UI
            isRepawned = true;
            print($"Player hp : {hp}");
            // �̶� hp�� 0�� �ƴٸ�
            // "(�����̸�)�� ���ߴٸ� �־��ش�"
            // ���� ��� �̸�ǥ�� ����ش�
            // ������ Ÿ���� ������ 
            // �ش� ���� ������Ʈ�� ���ش�
            photonView.RPC("RPCDie", RpcTarget.All);
        }


        if (isRepawned == true)
        {
            currentTime2 += Time.deltaTime;
            killMsgBox.SetActive(true);
            killMsgtxt.text = weaponName + "�� ���ߴ�!";
            if (currentTime2 > respawnTime)
            {
                killMsgBox.SetActive(false);
                isRepawned = false;
                currentTime2 = 0;
            }
        }
    }

    [PunRPC]
    public void RPCDie()
    {
        body.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        inkTank.gameObject.SetActive(false);

        currentTime += Time.deltaTime;
        if (currentTime > respawnTime)
        {
            
            if (name.Contains("Pink"))
            {
                gameObject.transform.position = pink_RespawnPoint.position;
            }

            else if(name.Contains("Blue"))
            {
                gameObject.transform.position = pink_RespawnPoint.position;

            }

            body.gameObject.SetActive(true);
            weapon.gameObject.SetActive(true);
            inkTank.gameObject.SetActive(true);
            currentTime = 0;
            hp = 3;
        }
    }
}
