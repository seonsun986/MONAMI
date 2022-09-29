using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_HP : MonoBehaviourPun
{
    //화면 피격 시 스크린 메테리얼
    public Material screenMaterial;

    Transform pink_RespawnPoint;
    Transform blue_RespawnPoint;

    public int hp = 1;
    float currentTime;
    public float respawnTime = 3;

    // 카메라는 살아있어야한다
    public GameObject body;
    public GameObject weapon;
    public GameObject inkTank;
    void Start()
    {
        //풀 스크린 가져오기
        screenMaterial = Resources.Load<Material>("Voronoi_FullScreen_tut");

        pink_RespawnPoint = GameObject.Find("PinkTeam_Respawn").transform;
        blue_RespawnPoint = GameObject.Find("BlueTeam_Respawn").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            print("들어왔다!");
            screenMaterial.SetFloat("_FullscreenIntensity", 0.5f);
        }
        if (hp <=0)
        {
            print($"Player hp : {hp}");
            photonView.RPC("RPCDie", RpcTarget.All);
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
            hp = 1;
        }
    }
}
