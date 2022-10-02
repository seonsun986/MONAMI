using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class Result : MonoBehaviourPun
{
    public Material screenMaterial;

    double pink;
    double blue;
    public Text pinkRatio;
    public Text blueRatio;        // 슬라이더의 총 value는 1이다
    public Text resultTxt;
    public Slider slider;
    public Animator pink_anim;
    public Animator blue_anim;
    public AudioSource audio1;
    public AudioSource audio2;

    public Image textureImage;
    public float currentTime;              // 이걸로 소리 및 고양이의 애니메이션 조절하자
    public float resultTime = 3.5f;
    private void Awake()
    {
        screenMaterial.SetFloat("_FullscreenIntensity", 0f);
    }
    void Start()
    {
        resultTxt.gameObject.SetActive(false);

        // 방장 이름으로 캡쳐파일 불러오기
        byte[] textureBytes = File.ReadAllBytes(Application.dataPath + "/ScreenShot/" + PhotonNetwork.MasterClient.NickName + ".png");
        if (textureBytes.Length > 0)
        {
            Texture2D loadedTexture = new Texture2D(0, 0);
            loadedTexture.LoadImage(textureBytes);
            textureImage.sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), Vector2.zero);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime> 1f)
        {
            audio1.gameObject.SetActive(true);
        }
        if(currentTime>resultTime - 0.4f)
        {
            

            // 마스터만 블루 포인트와 레드포인트 다른 사람들에게 넘겨준다
            if (PhotonNetwork.IsMasterClient)
            {
                pink = (double)DataManager.instance.Pink_point;
                blue = (double)DataManager.instance.Blue_point;
                photonView.RPC("RPCResult", RpcTarget.All, pink, blue);
            }

            // 핑크팀이 이겼을 때
            resultTxt.gameObject.SetActive(true);
            if (pink_anim.GetCurrentAnimatorStateInfo(0).IsName("Victory"))
            {
                if (DataManager.instance.id == 1)
                {
                    resultTxt.text = "승리";
                }
                else
                {
                    resultTxt.text = "패배";
                }
            }
            else
            {
                if (DataManager.instance.id == 1)
                {
                    resultTxt.text = "패배";
                }
                else
                {
                    resultTxt.text = "승리";
                }
            }

        }
        if (currentTime > resultTime)
        {
            audio2.gameObject.SetActive(true);
        }

    }

    [PunRPC]
    public void RPCResult(double pink, double blue)
    {
        pinkRatio.text = (((pink / (pink + blue)) * 100)).ToString("F1") + "%";
        //pinkRatio.text = string.Format("{0:0.#}", (pink / (pink + blue)) * 100) + "%";
        blueRatio.text = (((blue / (pink + blue)) * 100)).ToString("F1") + "%";
        //blueRatio.text = string.Format("{0:0.#}", (blue / (pink + blue)) * 100) + "%";
        slider.value = (float)(pink / (pink + blue));

        // 핑크팀이 이겼을 때
        if (pink / (pink + blue) > blue / (pink + blue))
        {
            pink_anim.SetTrigger("Victory");
            blue_anim.SetTrigger("Defeat");
        }
        // 파랑팀이 이겼을 때
        else
        {
            blue_anim.SetTrigger("Victory");
            pink_anim.SetTrigger("Defeat");
        }

        print((((pink / (pink + blue)) * 100)).ToString("F1") + "%");
    }
}
