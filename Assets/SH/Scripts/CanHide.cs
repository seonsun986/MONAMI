using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanHide : MonoBehaviour
{
    [Header("숨는 키")]
    [SerializeField]
    private KeyCode hideKey = KeyCode.LeftShift;
    [SerializeField]
    private bool canHide;

    public Color[] colors;
    public string[] texts;

    public Texture2D imageMap;

    public float rayDistance = 20;
    public RawImage rawImage;
    public GameObject sphere;       // 오징어로 변했을 때 공격당하기 위함
    public GameObject body;
    public GameObject weapon;
    // Ray를 바닥으로 쏜다
    // 해당 Ray에 대한 Pixel을 구한다.
    
    void Start()
    {
        
        //sphere.SetActive(false);
    }

    RaycastHit hitInfo;
    string colorName;
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hitInfo;
        if (Input.GetKeyDown(hideKey))
        {
           
        }

        if (Input.GetKey(hideKey) && Physics.Raycast(ray, out hitInfo))
        {

            // ID로 접근 --> 쉐이더 그래프 ID -> int값 출력
            Paintable paintable = hitInfo.transform.GetComponent<Paintable>();
            if(paintable != null)
            {
                RenderTexture render = paintable.getMask();
                Texture2D text = RenderTextureTo2DTexture(render);
                Vector2 pixelUV = hitInfo.textureCoord;
                pixelUV.x *= text.width;
                pixelUV.y *= text.height;
                Color color = text.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                print("뽑아온 색깔 #" + ColorUtility.ToHtmlStringRGB(color));
                print("RGB 색깔 : " + color.r + ", " + color.g + ", " + color.b);

                string getColor = ColorUtility.ToHtmlStringRGB(color);


            }
            
            //    // 내 색깔일 때
            //    if (getColor.Substring(0,1)== "6" || getColor.Substring(0, 1) == "7")
            //    {
            //        canHide = true;
            //    }
            //    // 상대편 색깔일때
            //    else if(getColor.Substring(0, 1) == "F")
            //    {
            //        print("못 숨는다!");
            //        canHide = false;
            //    }
            //    else
            //    {
            //        canHide = false;
            //    }
            //    print(canHide);
            //}
            //if(Input.GetKeyUp(hideKey))
            //{
            //    canHide = false;
            //}

            //if(canHide == true)
            //{
            //    // 숨을 수 있을 때
            //    CanHideP();
            //}
            //else
            //{
            //    // 숨을 수 없을 때
            //    CanNotHide();
            //}
        }
    }

    private Texture2D RenderTextureTo2DTexture(RenderTexture rt)
    {
        var texture = new Texture2D(rt.width, rt.height, rt.graphicsFormat, 0);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();

        RenderTexture.active = null;

        return texture;
    }

    private Texture2D TextureToTexture2d(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }

    void CanHideP()
    {
        // 오징어로 변해도 공격당할 수 있게 함
        sphere.gameObject.SetActive(true);
        // 플레이어 안보이게 함
        body.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);

        // 만약 플레이어가 롤러라면
        if (gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 13;
        }

    }

    
    void CanNotHide()
    {
        sphere.gameObject.SetActive(false);
        body.gameObject.SetActive(true);
        weapon.gameObject.SetActive(true);

        // 만약 플레이어가 롤러라면
        if(gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 8;
        }
    }
}
