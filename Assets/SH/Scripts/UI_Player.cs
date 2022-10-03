using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UI_Player : MonoBehaviourPun
{
    public static UI_Player Instance;
    // id 1~3�� ��ũ��
    // id 4~6�� ������
    // �ڽ��� ������ ���⿡ ���� �̹���UI�� Canvas�ڽ����� �����ϰ� �ʹ�
    public GameObject[] weapon_UI;     // 1~3 ��ũ(����, �ѷ�, ����) 4~6 ����(����, �ѷ�, ����)
    public GameObject[] UI = new GameObject[6];
    public Vector3[] UI_transform;
    public GameObject start_UI;
    public GameObject blue_bar;
    public GameObject pink_bar;
    public GameObject countDown;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(start_UI.activeSelf == false)
        {
            blue_bar.SetActive(true);
            pink_bar.SetActive(true);
            countDown.SetActive(true);
            photonView.RPC("RPC_UI", RpcTarget.All, DataManager.instance.id, DataManager.instance.weaponName);

        }
    }

    [PunRPC]
    public void RPC_UI(int id, string weaponName)
    {

        // ���̵� 1~3�϶�
        if (id >= 1 && id <= 3)
        {
            if (weaponName == "Shooter")
            {
                // �����Ѱ� ���Ͷ��
                UI[id-1] = Instantiate(weapon_UI[0]);
            }
            else if (weaponName == "Roller")
            {
                UI[id-1] = Instantiate(weapon_UI[1]);
            }
            else if (weaponName == "Charger")
            {
                UI[id-1] = Instantiate(weapon_UI[2]);
            }

            // 0���� �����ϹǷ�
            UI[id - 1].transform.SetParent(GameObject.Find("Canvas").transform,false);
            UI[id - 1].transform.position = UI_transform[id - 1];
        }

        // ���̵� 4~6�϶�
        else
        {
            if (weaponName == "Shooter")
            {
                // �����Ѱ� ���Ͷ��
                UI[id] = Instantiate(weapon_UI[3]);
            }
            else if (weaponName == "Roller")
            {
                UI[id] = Instantiate(weapon_UI[4]); ;
            }
            else if (weaponName == "Charger")
            {
                UI[id] = Instantiate(weapon_UI[5]);
            }

            // 0���� �����ϹǷ�
            UI[id - 1].transform.parent = GameObject.Find("Canvas").transform;
            UI[id - 1].GetComponent<RectTransform>().anchoredPosition = UI_transform[id - 1];
        }
    }

}