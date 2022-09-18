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
    public GameObject inkTank;
    public GameObject UI_chageInk;
    public float[] rgb = new float[3];

    // 오징어 나타내기 위한 것들
    CharacterController cc;
    public GameObject squid;
    // Ray를 바닥으로 쏜다
    // 해당 Ray에 대한 Pixel을 구한다.

    // 숨을 때 총알 count 0으로 되살리기 위한 것
    // 숨을때 일단 충전 UI는 무조건 나와야한다
    // 쏠 수 없을 때(canShoot == false)
    // 1초에 30개씩 총알 충전되도록 한다
    // maxcount보다 count가 많아지면 maxCount로 다시 하게 한다
    // 필요속성 : canShoot, 충전개수, maxCount로
    void Start()
    {
        cc = GetComponent<CharacterController>();
        UI_chageInk.SetActive(false);
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
            if (paintable != null)
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
                rgb[0] = color.r;
                rgb[1] = color.g;
                rgb[2] = color.b;


            }

            // 내 색깔일 때
            if (rgb[0] < 1.1f && rgb[0] > 0.9f && rgb[1] == 0 && rgb[2] > 0.4f && rgb[2] < 0.6f)
            {
                canHide = true;
            }
            // 상대편 색깔일때
            else
            {
                canHide = false;
            }
        }
        if (Input.GetKeyUp(hideKey))
        {
            canHide = false;
            UI_chageInk.SetActive(false);
        }

        if (canHide == true)
        {
            // 숨을 수 있을 때
            CanHideP();
        }
        else
        {
            // 숨을 수 없을 때
            CanNotHide();

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



    void CanHideP()
    {
        // 오징어로 변해도 공격당할 수 있게 함
        // 잉크 충전 UI킨다
        UI_chageInk.SetActive(true);
        sphere.gameObject.SetActive(true);
        // 플레이어 안보이게 함
        body.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        inkTank.SetActive(false);

        // 만약 플레이어가 롤러라면
        if (gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 13;
        }
        else if(gameObject.name.Contains("Shooter"))
        {
            PlayerMovement pm = GetComponent<PlayerMovement>();
            pm.run = true;
            PlayerShooter ps = GetComponent<PlayerShooter>();
            // 총알 충전을 위한 함수
            ps.ChargeInk();
            print("잉크 충전된다!");
        }

        // 숨을 때 바닥에 닫는다면
        // 오징어가 보이지 않게한다
        // 바닥에 닿았는지 확인하기 위한 RAY
        Ray groundRay = new Ray(transform.position, transform.up * -1);
        RaycastHit hitGround;
        if (Physics.Raycast(groundRay, out hitGround))
        {
            float distance = Vector3.Distance(transform.position, hitGround.point);
            // 바닥에 닿아있다면 
            // 아무것도 안보이게 한다
            // 충전을 위한 UI는 보이게한다.
            if (distance < 0.5f)
            {
                squid.SetActive(false);
            }
            else
            {
                squid.SetActive(true);
                //squid.transform.forward = transform.forward;

            }
            print("길이 : " + distance);
        }

    }

    
    void CanNotHide()
    {
        sphere.gameObject.SetActive(false);
        body.gameObject.SetActive(true);
        weapon.gameObject.SetActive(true);
        inkTank.SetActive(true);

        // 만약 플레이어가 롤러라면
        if(gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 8;
        }
        squid.SetActive(false);
    }
}
