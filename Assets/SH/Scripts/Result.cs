using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class Result : MonoBehaviourPun
{
    double pink;
    double blue;
    public Text pinkRatio;
    public Text blueRatio;        // �����̴��� �� value�� 1�̴�
    public Slider slider;
    public Animator pink_anim;
    public Animator blue_anim;


    public Image textureImage;
    private void Awake()
    {
        
    }
    void Start()
    {
        // �����͸� ��� ����Ʈ�� ��������Ʈ �ٸ� ����鿡�� �Ѱ��ش�
        if(PhotonNetwork.IsMasterClient)
        {
            pink = (double)DataManager.instance.Pink_point;
            blue = (double)DataManager.instance.Blue_point;
            photonView.RPC("RPCResult", RpcTarget.All, pink, blue);
        }

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

    }

    [PunRPC]
    public void RPCResult(double pink, double blue)
    {
        pinkRatio.text = (((pink / (pink + blue)) * 100)).ToString("F1") + "%";
        //pinkRatio.text = string.Format("{0:0.#}", (pink / (pink + blue)) * 100) + "%";
        blueRatio.text = (((blue / (pink + blue)) * 100)).ToString("F1") + "%";
        //blueRatio.text = string.Format("{0:0.#}", (blue / (pink + blue)) * 100) + "%";
        slider.value = (float)(pink / (pink + blue));

        if (pink / (pink + blue) > blue / (pink + blue))
        {
            pink_anim.SetTrigger("Victory");
            blue_anim.SetTrigger("Defeat");
        }
        else
        {
            blue_anim.SetTrigger("Victory");
            pink_anim.SetTrigger("Defeat");
        }

        print((((pink / (pink + blue)) * 100)).ToString("F1") + "%");
    }
}
