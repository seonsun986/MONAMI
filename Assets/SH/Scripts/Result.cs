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
    public Text blueRatio;        // �����̴��� �� value�� 1�̴�
    public Text resultTxt;
    public Slider slider;
    public Animator pink_anim;
    public Animator blue_anim;
    public AudioSource audio1;
    public AudioSource audio2;

    public Image textureImage;
    public float currentTime;              // �̰ɷ� �Ҹ� �� ������� �ִϸ��̼� ��������
    public float resultTime = 3.5f;
    private void Awake()
    {
        screenMaterial.SetFloat("_FullscreenIntensity", 0f);
    }
    void Start()
    {
        resultTxt.gameObject.SetActive(false);

        // ���� �̸����� ĸ������ �ҷ�����
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
            

            // �����͸� ��� ����Ʈ�� ��������Ʈ �ٸ� ����鿡�� �Ѱ��ش�
            if (PhotonNetwork.IsMasterClient)
            {
                pink = (double)DataManager.instance.Pink_point;
                blue = (double)DataManager.instance.Blue_point;
                photonView.RPC("RPCResult", RpcTarget.All, pink, blue);
            }

            // ��ũ���� �̰��� ��
            resultTxt.gameObject.SetActive(true);
            if (pink_anim.GetCurrentAnimatorStateInfo(0).IsName("Victory"))
            {
                if (DataManager.instance.id == 1)
                {
                    resultTxt.text = "�¸�";
                }
                else
                {
                    resultTxt.text = "�й�";
                }
            }
            else
            {
                if (DataManager.instance.id == 1)
                {
                    resultTxt.text = "�й�";
                }
                else
                {
                    resultTxt.text = "�¸�";
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

        // ��ũ���� �̰��� ��
        if (pink / (pink + blue) > blue / (pink + blue))
        {
            pink_anim.SetTrigger("Victory");
            blue_anim.SetTrigger("Defeat");
        }
        // �Ķ����� �̰��� ��
        else
        {
            blue_anim.SetTrigger("Victory");
            pink_anim.SetTrigger("Defeat");
        }

        print((((pink / (pink + blue)) * 100)).ToString("F1") + "%");
    }
}
