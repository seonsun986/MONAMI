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
    // 죽음 당한 닉네임 있는 게임 오브젝트
    public GameObject killNameBox;
    // (무기이름)에게 당했다! 넣어줄 텍스트
    public Text killMsgtxt;
    // 죽음 당한 닉네임 넣어줄 텍스트
    public Text killNametxt;
    bool isRepawned;
    float currentTime2;
    string teamName;

    // 소리
    public AudioSource deathSound;
    void Start()
    {

        hp = 10;
        // 풀 스크린 가져오기
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
            // 아이디 바꿔주기!!
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
            // 리스폰 될때까지 당한 무기 알려주는 UI
            isRepawned = true;
            print($"Player hp : {hp}");
            // 이때 hp가 0이 됐다면
            // "(무기이름)에 당했다를 넣어준다"
            // 죽인 사람 이름표를 띄워준다
            // 리스폰 타임이 지나면 
            // 해당 게임 오브젝트를 꺼준다
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
            // 영어 무기 한글로 바꿔주기
            if(weapon.name == "Shooter")
            {
                killMsgtxt.text = "슈터에 당했다!";
            }
            else if(weapon.name == "Roller")
            {
                killMsgtxt.text = "롤러에 당했다!";

            }
            else
            {
                killMsgtxt.text = "차저에 당했다!";
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

    // 1. hp가 0이 되는 순간
    // 2. 나한테 해당하는 UI를 흑백으로 바꾸고 싶다
    // 3. 그 위치에 X를 배치시키고 싶다(Instantiate로 하자)
    // 4. 리스폰이 false가 되면 X를 끄고 흑백을 다시 컬러로 바꾸고 싶다
    // 5. 한번만 실행하고 싶다
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
