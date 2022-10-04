using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

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

    public string[] nickname = new string[6];
    int i;
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
    int count2;
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 1f)
        {
            audio1.gameObject.SetActive(true);
        }
        if (currentTime > resultTime - 0.4f)
        {


            // �����͸� ��� ����Ʈ�� ��������Ʈ �ٸ� ����鿡�� �Ѱ��ش�
            if (PhotonNetwork.IsMasterClient && count2<1)
            {
                pink = (double)DataManager.instance.Pink_point;
                blue = (double)DataManager.instance.Blue_point;
                photonView.RPC("RPCResult", RpcTarget.All, pink, blue);
                count2++;
            }

            // ��ũ���� �̰��� ��
            resultTxt.gameObject.SetActive(true);
            if (pink_anim.GetCurrentAnimatorStateInfo(0).IsName("Victory"))
            {
                if (DataManager.instance.id >= 1 && DataManager.instance.id <=3)
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
                if (DataManager.instance.id >= 4 && DataManager.instance.id <=6)
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
    public Transform spawnPoint;
    int count = 0;
    float currentTime2;
    [PunRPC]
    public void RPCResult(double pink, double blue)
    {
        currentTime2 += Time.deltaTime;
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

            if (currentTime2 > 1f && count <1)
            {
                Create("Pink", 1, DataManager.instance.resultInfos[0].weapon, DataManager.instance.resultInfos[0].nickName);
                Create("Pink", 2, DataManager.instance.resultInfos[1].weapon, DataManager.instance.resultInfos[1].nickName);
                Create("Pink", 3, DataManager.instance.resultInfos[2].weapon, DataManager.instance.resultInfos[2].nickName);
                count++;
            }

        }

        // �Ķ����� �̰��� ��
        else
        {
            blue_anim.SetTrigger("Victory");
            pink_anim.SetTrigger("Defeat");

            if (currentTime2 > 1 && count<1)
            {
                Create("Blue", 1, DataManager.instance.resultInfos[0].weapon, DataManager.instance.resultInfos[0].nickName);
                Create("Blue", 2, DataManager.instance.resultInfos[1].weapon, DataManager.instance.resultInfos[1].nickName);
                Create("Blue", 3, DataManager.instance.resultInfos[2].weapon, DataManager.instance.resultInfos[2].nickName);
                count++;
            }

        }

        print((((pink / (pink + blue)) * 100)).ToString("F1") + "%");
    }

    void Create(string team, int id, string weaponName, string nickName)
    {
        if (weaponName == "Shooter")
        {
            GameObject shooter = PhotonNetwork.Instantiate("Shooter_" + team + "_Ending", spawnPoint.position, Quaternion.Euler(0, 157.331f, 0));
            Ending ending = shooter.GetComponent<Ending>();
            ending.nickname.text = nickName;          
            Animation anim = shooter.GetComponent<Animation>();
            // �ٲ���Ѵ�!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (id == 1 || id == 4)
            {
                anim.Play("First");
            }
            else if(id ==2|| id ==5)
            {
                anim.Play("Second");
            }
            else
            {
                anim.Play("Third");
            }

        }
        else if (weaponName == "Roller")
        {
            GameObject Roller = PhotonNetwork.Instantiate("Roller_" + team + "_Ending", spawnPoint.position, Quaternion.Euler(0, 157.331f, 0));
            Ending ending = Roller.GetComponent<Ending>();
            ending.nickname.text = nickName;
            Animation anim = Roller.GetComponent<Animation>();
            if (id == 1 || id == 4)
            {
                anim.Play("First");
            }
            else if (id == 2 || id == 5)
            {
                anim.Play("Second");
            }
            else
            {
                anim.Play("Third");
            }
        }
        else
        {
            GameObject Charger = PhotonNetwork.Instantiate("Charger_" + team + "_Ending", spawnPoint.position, Quaternion.Euler(0, 157.331f, 0));
            Ending ending = Charger.GetComponent<Ending>();
            ending.nickname.text = nickName;
            Animation anim = Charger.GetComponent<Animation>();
            if (id == 1 || id == 4)
            {
                anim.Play("First");
            }
            else if (id == 2 || id == 5)
            {
                anim.Play("Second");
            }
            else
            {
                anim.Play("Third");
            }
        }
    }

}