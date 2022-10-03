using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UI_Player : MonoBehaviourPun
{
    public static UI_Player Instance;
    // id 1~3은 핑크팀
    // id 4~6은 블루팀
    // 자신이 선택한 무기에 따라 이미지UI를 Canvas자식으로 생성하고 싶다
    public GameObject[] weapon_UI;     // 1~3 핑크(슈터, 롤러, 차저) 4~6 블루(슈터, 롤러, 차저)
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
    int count;
    void Update()
    {
        if (start_UI.activeSelf == false && count<1)
        {
            blue_bar.SetActive(true);
            pink_bar.SetActive(true);
            countDown.SetActive(true);


            if(PhotonNetwork.IsMasterClient)
            {
                for(int i = 0; i < GameManager.Instance.players.Count; i++)
                {
                    SSH_Player player = GameManager.Instance.players[i].GetComponent<SSH_Player>();
                    photonView.RPC("RPC_UI", RpcTarget.All, player.id, player.weaponName);
                }

            }
            count++;
        }
    }

    [PunRPC]
    public void RPC_UI(int id, string weaponName)
    {

        // 아이디가 1~3일때
        if (id >= 1 && id <= 3)
        {
            if (weaponName == "Shooter")
            {
                // 선택한게 슈터라면
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

            // 0부터 시작하므로
            UI[id - 1].transform.SetParent(GameObject.Find("Canvas").transform,false);
            UI[id - 1].transform.position = UI_transform[id - 1];
        }

        // 아이디가 4~6일때
        else if(id >=4 && id <=6)
        {
            if (weaponName == "Shooter")
            {
                // 선택한게 슈터라면
                UI[id - 1] = Instantiate(weapon_UI[3]);
            }
            else if (weaponName == "Roller")
            {
                UI[id - 1] = Instantiate(weapon_UI[4]); ;
            }
            else if (weaponName == "Charger")
            {
                UI[id - 1] = Instantiate(weapon_UI[5]);
            }

            // 0부터 시작하므로
            UI[id - 1].transform.parent = GameObject.Find("Canvas").transform;
            UI[id - 1].GetComponent<RectTransform>().anchoredPosition = UI_transform[id - 1];
        }
    }

}
