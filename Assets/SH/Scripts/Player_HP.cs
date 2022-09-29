using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Player_HP : MonoBehaviourPun
{
    // 화면 피격시 스크린 머티리얼
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

    // 플레이어의 아이디를 넣어주기 위한 변수
    public string weaponName;

    // (무기이름)에게 당했다! 있는 게임 오브젝트
    public GameObject killMsgBox;
    // (무기이름)에게 당했다! 넣어줄 텍스트
    public Text killMsgtxt;
    bool isRepawned;
    float currentTime2;

    void Start()
    {
        // 풀 스크린 가져오기
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
            // 리스폰 될때까지 당한 무기 알려주는 UI
            isRepawned = true;
            print($"Player hp : {hp}");
            // 이때 hp가 0이 됐다면
            // "(무기이름)에 당했다를 넣어준다"
            // 죽인 사람 이름표를 띄워준다
            // 리스폰 타임이 지나면 
            // 해당 게임 오브젝트를 꺼준다
            photonView.RPC("RPCDie", RpcTarget.All);
        }


        if (isRepawned == true)
        {
            currentTime2 += Time.deltaTime;
            killMsgBox.SetActive(true);
            killMsgtxt.text = weaponName + "에 당했다!";
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
