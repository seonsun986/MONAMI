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
    // ���� ���� �г��� �ִ� ���� ������Ʈ
    public GameObject killNameBox;
    // (�����̸�)���� ���ߴ�! �־��� �ؽ�Ʈ
    public Text killMsgtxt;
    // ���� ���� �г��� �־��� �ؽ�Ʈ
    public Text killNametxt;
    bool isRepawned;
    float currentTime2;
    string teamName;

    // �Ҹ�
    public AudioSource deathSound;
    void Start()
    {

        hp = 10;
        // Ǯ ��ũ�� ��������
        screenMaterial = Resources.Load<Material>("Voronoi_FullScreen");
        screenMaterial.SetFloat("_FullscreenIntensity", 0f);
        if (gameObject.name.Contains("Blue"))
        {
            Color color = new Color(1, 0, 0.5f);
            screenMaterial.SetColor("_Color", color);
        }
        else if(gameObject.name.Contains("Pink"))
        {
            Color color = new Color(0, 0.5f, 1);
            screenMaterial.SetColor("_Color", color);
        }

        pink_RespawnPoint = GameObject.Find("PinkTeam_Respawn").transform;
        blue_RespawnPoint = GameObject.Find("BlueTeam_Respawn").transform;
        killMsgBox.SetActive(false);
        killNameBox.SetActive(false);

        if(photonView.IsMine)
        {
            // ���̵� �ٲ��ֱ�!!
            if (DataManager.instance.id >= 1 && DataManager.instance.id <=3)
            {
                teamName = "Pink";
            }
            else
            {
                teamName = "Blue";
            }
        }
    }
    int count2;
    void Update()
    {
        
        if (!photonView.IsMine) return;
        if(hp >=6 && hp < 10)
        {
            screenMaterial.SetFloat("_FullscreenIntensity", 0.1f);
        }
        else if(hp <=5 && hp>=1)
        {
            screenMaterial.SetFloat("_FullscreenIntensity", 0.2f);
        }
        else if (hp <=0)
        {
            hp = 0;
            if(!deathSound.isPlaying && count2<1)
            {
                deathSound.Play();
                count2++;
            }
            screenMaterial.SetFloat("_FullscreenIntensity", 0.5f);
            // ������ �ɶ����� ���� ���� �˷��ִ� UI
            isRepawned = true;
            print($"Player hp : {hp}");
            // �̶� hp�� 0�� �ƴٸ�
            // "(�����̸�)�� ���ߴٸ� �־��ش�"
            // ���� ��� �̸�ǥ�� ����ش�
            // ������ Ÿ���� ������ 
            // �ش� ���� ������Ʈ�� ���ش�
            //photonView.RPC("RPCDie", RpcTarget.All);

            

            if (count<1)
            {
                SSH_Player p = GetComponent<SSH_Player>();
                photonView.RPC("UI_Die", RpcTarget.All, p.id, p.weaponName, teamName, isRepawned);
                count++;
            }

            currentTime += Time.deltaTime;
            if (currentTime > respawnTime)
            {
                if (name.Contains("Pink"))
                {
                    gameObject.transform.position = pink_RespawnPoint.position;
                }

                else if (name.Contains("Blue"))
                {
                    gameObject.transform.position = blue_RespawnPoint.position;
                }

                currentTime = 0;
                count = 0;
                count2 = 0;
                hp = 10;

                killMsgBox.SetActive(false);
                killNameBox.SetActive(false);
                isRepawned = false;
                currentTime2 = 0;
                screenMaterial.SetFloat("_FullscreenIntensity", 0f);

                SSH_Player p = GetComponent<SSH_Player>();
                photonView.RPC("UI_Die", RpcTarget.All, p.id, p.weaponName, teamName, isRepawned);
            }
        }


        if (isRepawned == true)
        {
            currentTime2 += Time.deltaTime;
            killMsgBox.SetActive(true);
            killNameBox.SetActive(true);
            // ���� ���� �ѱ۷� �ٲ��ֱ�
            if(weapon.name == "Shooter")
            {
                killMsgtxt.text = "���Ϳ� ���ߴ�!";
            }
            else if(weapon.name == "Roller")
            {
                killMsgtxt.text = "�ѷ��� ���ߴ�!";

            }
            else
            {
                killMsgtxt.text = "������ ���ߴ�!";
            }
            killNametxt.text = DataManager.instance.nickname;
        }
    }

    private void LateUpdate()
    {
        if(isRepawned)
        {
            RPCDie(!isRepawned);
        }
    }

    [PunRPC]
    public void RPCDie(bool isActive)
    {
        body.gameObject.SetActive(isActive);
        weapon.gameObject.SetActive(isActive);
        inkTank.gameObject.SetActive(isActive);
       
    }

    // 1. hp�� 0�� �Ǵ� ����
    // 2. ������ �ش��ϴ� UI�� ������� �ٲٰ� �ʹ�
    // 3. �� ��ġ�� X�� ��ġ��Ű�� �ʹ�(Instantiate�� ����)
    // 4. �������� false�� �Ǹ� X�� ���� ����� �ٽ� �÷��� �ٲٰ� �ʹ�
    // 5. �ѹ��� �����ϰ� �ʹ�
    int count;
    public GameObject X;
    [PunRPC]
    public void UI_Die(int id, string weaponName, string team, bool _isRepawned)
    {
        isRepawned = _isRepawned;
        UI_Player.Instance.UI[id - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI_" + weaponName + "_BK");
        //GameObject UI_X = Instantiate(X);
        //UI_X.transform.SetParent(GameObject.Find("Canvas").transform);
        //UI_X.transform.position = UI_Player.Instance.UI[id - 1].transform.position - new Vector3(0, -180, 0);

        if(_isRepawned == false)
        {
            //Destroy(UI_X);
            UI_Player.Instance.UI[id - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/UI_" + weaponName + "_" + team);
        }

    }
}
