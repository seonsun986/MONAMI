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

    [Header("영역 판정을 위한 변수들")]
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
            // 10초 이내일 경우 0을 붙여준다
            seconds = seconds.Length == 1 ? seconds = "0" + seconds : seconds;

            count.text = minute + " : " + seconds;
            // 60초 짜리는 안띄우기

            
        }

        if (countTime <= 0)
        {
            count.text = "0 : 00";
            Time.timeScale = 0;         // 게임 정지
            gameEndImg.SetActive(true);


            //// 영역판정 시작!
            //paintable paint = plane.getcomponent<paintable>();

            //rendertexture resultplane = paint.getmask();
            //texture2d resuletexture = rendertextureto2dtexture(resultplane);
            //for (int i = 0; i < resuletexture.width; i++)
            //{
            //    for (int j = 0; j < resuletexture.height; j++)
            //    {
            //        color color = resuletexture.getpixel(i, j);
            //        // 컬러가 핑크에 가까울 때
            //        if (color.r > pink_r - 0.3f && color.r < pink_r + 0.3f &&
            //           color.g > pink_g - 0.3f && color.g < pink_g + 0.3f &&
            //           color.b > pink_b - 0.3f && color.b < pink_b + 0.3f)
            //        {
            //            // 핑크 포인트를 올려준다
            //            pink_point++;
            //        }
            //        // 컬러가 블루에 가까울 때
            //        else if (color.r > blue_r - 0.3f && color.r < blue_r + 0.3f &&
            //                color.g > blue_g - 0.3f && color.g < blue_g + 0.3f &&
            //                color.b > blue_b - 0.3f && color.b < blue_b + 0.3f)
            //        {
            //            // 블루 포인트를 올려준다
            //            blue_point++;
            //        }
            //    }
            //}
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("버튼을 눌렀습니다.");
            Paintable paint = plane.GetComponent<Paintable>();

            RenderTexture resultTexture = paint.getMask();

            SaveRenderTextureToPNG(resultTexture, "Assets/Python/images/", "result");
        }

    }

    //다른 애들이 받기위한 변수
    float time;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(PhotonNetwork.IsMasterClient)
        {        
            stream.SendNext(countTime);
        }

        // 다른 사람이라면 데이터 받기
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
        // 경로가 안들어오면 종료
        if (string.IsNullOrEmpty(directroyPath)) return;

        // 디렉토리가 없으면 생성
        if (Directory.Exists(directroyPath) == false)
        {
            Debug.Log("디렉토리가 없습니다." + "\n" + "생성완료");
            Directory.CreateDirectory(directroyPath);
        }

        // Texture -> Texture2D로 변환
        int width = texture.width;
        int height = texture.height;

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture copiedRenderTexture = new RenderTexture(width, height, 0);

        // copiedRenderTexture 로 texture를 복사
        Graphics.Blit(texture, copiedRenderTexture);

        RenderTexture.active = copiedRenderTexture;

        // TextureFormat에서 RGB24 는 알파가 존재하지 않는다.
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);

        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRenderTexture;

        // Texture PNG bytes로 인코딩
        byte[] texturePNGBytes = texture2D.EncodeToPNG();

        // 경로 설정
        string filePath = directroyPath + fileName + ".png";

        // 파일 저장
        File.WriteAllBytes(filePath, texturePNGBytes);

        Debug.Log("파일 저장 완료");
    }


}
