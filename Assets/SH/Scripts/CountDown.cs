using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.IO;

public class CountDown : MonoBehaviourPun//, IPunObservable
{
    public float countTime = 300;

    public TextMeshProUGUI count;
    public GameObject gameEndImg;


    bool isStart = false;

    [Header("���� ������ ���� ������")]
    public GameObject plane;
    public int pink_Point;      // rgb 1 / 0 / 0.5
    float pink_R = 1;
    float pink_G = 0;
    float pink_B = 0.5f;
    public int blue_Point;      // rgb 0 / 0.5 / 1
    float blue_R = 0;
    float blue_G = 0.5f;
    float blue_B = 1;

    void Start()
    {
        
    }

    [PunRPC]
    void RpcStartCount()
    {
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart == false) return;


        if (countTime > 0)
        {
            countTime -= Time.deltaTime;
            string minute = Mathf.FloorToInt(countTime / 60).ToString();
            string seconds = (countTime % 60).ToString("F0");
            // 10�� �̳��� ��� 0�� �ٿ��ش�
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            count.text = minute + " : " + seconds;
            // 60�� ¥���� �ȶ���

            
        }

        if (countTime <= 0)
        {
            count.text = "0 : 00";
            Time.timeScale = 0;         // ���� ����
            gameEndImg.SetActive(true);


            //// �������� ����!
            //paintable paint = plane.getcomponent<paintable>();

            //rendertexture resultplane = paint.getmask();
            //texture2d resuletexture = rendertextureto2dtexture(resultplane);
            //for (int i = 0; i < resuletexture.width; i++)
            //{
            //    for (int j = 0; j < resuletexture.height; j++)
            //    {
            //        color color = resuletexture.getpixel(i, j);
            //        // �÷��� ��ũ�� ����� ��
            //        if (color.r > pink_r - 0.3f && color.r < pink_r + 0.3f &&
            //           color.g > pink_g - 0.3f && color.g < pink_g + 0.3f &&
            //           color.b > pink_b - 0.3f && color.b < pink_b + 0.3f)
            //        {
            //            // ��ũ ����Ʈ�� �÷��ش�
            //            pink_point++;
            //        }
            //        // �÷��� ��翡 ����� ��
            //        else if (color.r > blue_r - 0.3f && color.r < blue_r + 0.3f &&
            //                color.g > blue_g - 0.3f && color.g < blue_g + 0.3f &&
            //                color.b > blue_b - 0.3f && color.b < blue_b + 0.3f)
            //        {
            //            // ��� ����Ʈ�� �÷��ش�
            //            blue_point++;
            //        }
            //    }
            //}
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("��ư�� �������ϴ�.");
            Paintable paint = plane.GetComponent<Paintable>();

            RenderTexture resultTexture = paint.getMask();

            SaveRenderTextureToPNG(resultTexture, "Assets/Python/images/", "result");
        }

    }

    //�ٸ� �ֵ��� �ޱ����� ����
    float time;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(PhotonNetwork.IsMasterClient)
        {        
            stream.SendNext(countTime);
        }

        // �ٸ� ����̶�� ������ �ޱ�
        else
        {
            time = (float)stream.ReceiveNext();
        }
    }

    Texture2D texture;
    private Texture2D RenderTextureTo2DTexture(RenderTexture rt)
    {
        //var texture = new Texture2D(rt.width, rt.height, rt.graphicsFormat, 0);
        if (texture == null)
        {
            texture = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        }
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        //texture.Apply();

        RenderTexture.active = null;

        return texture;
    }

    public void SaveRenderTextureToPNG(RenderTexture texture, string directroyPath, string fileName)
    {
        // ��ΰ� �ȵ����� ����
        if (string.IsNullOrEmpty(directroyPath)) return;

        // ���丮�� ������ ����
        if (Directory.Exists(directroyPath) == false)
        {
            Debug.Log("���丮�� �����ϴ�." + "\n" + "�����Ϸ�");
            Directory.CreateDirectory(directroyPath);
        }

        // Texture -> Texture2D�� ��ȯ
        int width = texture.width;
        int height = texture.height;

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture copiedRenderTexture = new RenderTexture(width, height, 0);

        // copiedRenderTexture �� texture�� ����
        Graphics.Blit(texture, copiedRenderTexture);

        RenderTexture.active = copiedRenderTexture;

        // TextureFormat���� RGB24 �� ���İ� �������� �ʴ´�.
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);

        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRenderTexture;

        // Texture PNG bytes�� ���ڵ�
        byte[] texturePNGBytes = texture2D.EncodeToPNG();

        // ��� ����
        string filePath = directroyPath + fileName + ".png";

        // ���� ����
        File.WriteAllBytes(filePath, texturePNGBytes);

        Debug.Log("���� ���� �Ϸ�");
    }


}
