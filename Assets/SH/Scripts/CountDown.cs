using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System.IO;
using OpenCvSharp;
using UnityEngine.UI;

public class CountDown : MonoBehaviourPun//, IPunObservable
{
    public float countTime = 300;

    public TextMeshProUGUI count;
    public GameObject gameEndImg;
    //public GameObject screenShotCam;

    bool isStart = false;

    [Header("���� ������ ���� ������")]
    public GameObject plane;        // �ٴ�

    //SooYoung / 09.28 Update
    public GameObject time_Three;
    public GameObject time_Two;
    public GameObject time_One;

    void Start()
    {
        time_Three.SetActive(false);
        time_Two.SetActive(false);
        time_One.SetActive(false);
        //screenShotCam.SetActive(false);
    }

    [PunRPC]
    void RpcStartCount()
    {
        isStart = true;
    }

    // Update is called once per frame
    int changeScene;
    float currentTime2;
    public float changeTime1 = 1;
    public float changeTime2 = 3;

    void Update()
    {
        if (isStart == false) return;
        //���� ī��ƮŸ���� 3.5, 3 ���϶��
        if (countTime >= 3 && countTime <= 3.5f)
        {
            time_Three.SetActive(true);
        } 
        //ī��ƮŸ���� 2�̻��̰ų� 2.5���϶��
        if (countTime >= 2 && countTime <= 2.5f)
        {
            time_Three.SetActive(false);
            time_Two.SetActive(true);
        }
        if (countTime >= 1 && countTime <= 1.5f)
        {
            time_Two.SetActive(false);
            time_One.SetActive(true);
        }

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
        //������ ������!!!!!!!!!!
        if (countTime <= 0)
        {
            time_One.SetActive(false);

            currentTime2 += Time.deltaTime;
            count.text = "0 : 00";
            gameEndImg.SetActive(true);
            

            SaveRenderTextureToPNG(plane.GetComponent<Paintable>().getMask());      // �ٴ� ��������

            if(currentTime2 > changeTime1 && changeScene <1)
            {
                PhotonNetwork.LoadLevel("ResultScene");
                changeScene++;
            }

            //if(screenShotCount <1)
            //{
            //    if(PhotonNetwork.IsMasterClient)
            //    {
            //        screenShotCam.SetActive(true);
            //        screenShotCam.GetComponent<ScreenShot>().ScreenShotCam();
            //        screenShotCount++;
            //    }
               
            //}
            

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
        texture.ReadPixels(new UnityEngine.Rect(0, 0, rt.width, rt.height), 0, 0);
        //texture.Apply();

        RenderTexture.active = null;

        return texture;
    }

    public void SaveRenderTextureToPNG(RenderTexture texture)
    {

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

        texture2D.ReadPixels(new UnityEngine.Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRenderTexture;

        //// Texture PNG bytes�� ���ڵ�
        //byte[] texturePNGBytes = texture2D.EncodeToPNG();

        //File.WriteAllBytes(Application.dataPath + "/text.png", texturePNGBytes);


        //---------------------------------------//
        Mat image = OpenCvSharp.Unity.TextureToMat(texture2D);

        //RGB�� ����
        Cv2.CvtColor(image, image, ColorConversionCodes.BGR2RGB);

        Scalar red1 = new Scalar(255, 0, 0.5f * 255);
        Scalar red2 = new Scalar(255 * 0.7f, 0, 0.5f * 255 * 0.7f);

        Mat dst = new Mat();
        Cv2.InRange(image, red2, red1, dst);
        int a = Cv2.CountNonZero(dst);
        print("��ũ�� : " + a);
        DataManager.instance.Pink_point = a;
        red1 = new Scalar(0, 0.5f * 255, 255);
        red2 = new Scalar(0, 0.5f * 255 * 0.7f, 255 * 0.7f);

        dst = new Mat();
        Cv2.InRange(image, red2, red1, dst);
        a = Cv2.CountNonZero(dst);
        print("��簪 : " + a);
        DataManager.instance.Blue_point = a;


    }


}
