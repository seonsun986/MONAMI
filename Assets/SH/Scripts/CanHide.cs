using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CanHide : MonoBehaviourPun
{

    // 남의 잉크에서는 느려져야한다 
    // 그러면 내가 핑크팀인지 파란팀인지 알아야할텐데?
    // 아하.. 그럼 맨처음에 내가 무슨 팀인지 알게하자
    // Pink : 1,0,0.5 Blue : 0,0.5,1
    public Camera cam;
    [Header("숨는 키")]
    [SerializeField]
    private KeyCode hideKey = KeyCode.LeftShift;
    public bool canHide;

    public Color[] colors;
    public string[] texts;

    public float rayDistance = 20;
    //public RawImage rawImage;
    public GameObject sphere;       // 오징어로 변했을 때 공격당하기 위함
    public GameObject body;
    public GameObject weapon;
    public GameObject inkTank;
    public GameObject UI_chageInk;
    public GameObject UI_ChagerInkPaint;
    Color playerColor;
    float myColor_R;
    float myColor_G;
    float myColor_B;
    float enemyColor_R;
    float enemyColor_G;
    float enemyColor_B;
    public bool isInenemyColor;

    public float[] rgb = new float[3];

    // 오징어 나타내기 위한 것들
    CharacterController cc;
    public GameObject squid;

    //숨을 때 재생되는 파티클
    [SerializeField] ParticleSystem particle_Ink_Splash;
    //숨어있을 때 재생되는 파티클
    [SerializeField] ParticleSystem particle_Ink_Hiding;
    //오징어폼으로 변했을 때
    [SerializeField] GameObject VFX_squid_Speed;

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
        // 내가 핑크팀이라면
        if(name.Contains("Pink"))
        {
            // 적팀의 색깔은
            enemyColor_R = 0;
            enemyColor_G = 0.5f;
            enemyColor_B = 1;
        }

        // 내가 블루팀이라면
        if(name.Contains("Blue"))
        {
            // 적팀의 색깔은
            enemyColor_R = 1;
            enemyColor_G = 0;
            enemyColor_B = 0.5f;
        }

        cc = GetComponent<CharacterController>();
        UI_chageInk.SetActive(false);
        UI_ChagerInkPaint.SetActive(false);
        VFX_squid_Speed.SetActive(false);
        playerColor = UI_ChagerInkPaint.GetComponent<Image>().color;
        myColor_R = playerColor.r;
        myColor_G = playerColor.g;
        myColor_B = playerColor.b;
    }

    RaycastHit hitInfo;
    int count;
    void Update()
    {
        // 내것이라면
        if (photonView.IsMine)
        {
            Ray ray = new Ray(transform.position, transform.up * -1);
            RaycastHit hitInfo;


            // 상대편 잉크 위인지 확인하려면 update에서 해야한다
            // 2프레임에 한번씩만 판정하자
            // hideKey를 누르면 색깔 판정을 한다.

            if (Physics.Raycast(ray, out hitInfo))
            {
                Paintable paintable = hitInfo.transform.GetComponent<Paintable>();
                if (paintable != null)
                {
                    RenderTexture render = paintable.getMask();
                    if (render == null) print("render is null");
                    Texture2D text = RenderTextureTo2DTexture(render);
                    if (text == null) print("text is null");
                    Vector2 pixelUV = hitInfo.textureCoord;
                    pixelUV.x *= text.width;
                    pixelUV.y *= text.height;
                    Color color = text.GetPixel((int)pixelUV.x, (int)pixelUV.y);

                    string getColor = ColorUtility.ToHtmlStringRGB(color);
                    rgb[0] = color.r;
                    rgb[1] = color.g;
                    rgb[2] = color.b;

                }

                // 색깔판정을 했을 시 상대편 색깔일때
                if (rgb[0] < enemyColor_R + 0.3f && rgb[0] > enemyColor_R - 0.3f &&
                    rgb[1] < enemyColor_G + 0.3f && rgb[1] > enemyColor_G - 0.3f &&
                    rgb[2] > enemyColor_B - 0.3f && rgb[2] < enemyColor_B + 0.3f)
                {
                    isInenemyColor = true;
                }
                else
                {
                    isInenemyColor = false;
                }
                
            }





            if (Input.GetKey(hideKey) && Physics.Raycast(ray, out hitInfo))
            {
                //Zoom In
                cam.GetComponentInParent<CameraMovement>().zoomDistance = 4f;
                // ID로 접근 --> 쉐이더 그래프 ID -> int값 출력
                Paintable paintable = hitInfo.transform.GetComponent<Paintable>();
                if (paintable != null)
                {
                    RenderTexture render = paintable.getMask();
                    if (render == null) print("render is null");
                    Texture2D text = RenderTextureTo2DTexture(render);
                    if (text == null) print("text is null");
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

                
                // 색깔판정을 했을 시 내 색깔일 때
                if (rgb[0] < myColor_R + 0.4f && rgb[0] > myColor_R - 0.4f && 
                    rgb[1] < myColor_G + 0.4f && rgb[1] > myColor_G - 0.4f && 
                    rgb[2] > myColor_B - 0.4f && rgb[2] < myColor_B + 0.4f)
                {
                    canHide = true;

                }
                // 다른 색깔들일때
                else
                {
                    canHide = false;
                }
            }
            if (Input.GetKeyUp(hideKey))
            {
                //Zoom Out
                cam.GetComponentInParent<CameraMovement>().zoomDistance = 0f;
                if (canHide ==true)
                { particle_Ink_Splash.Play();}

                canHide = false;
                UI_chageInk.SetActive(false);
                UI_ChagerInkPaint.SetActive(false);
                VFX_squid_Speed.SetActive(false);
            }

            if (canHide == true)
            {
                
                // 숨을 수 있을 때
                CanHideP();
                photonView.RPC("RPCCanHide", RpcTarget.All);

            }

            // 숨을 수 없을 때
            else
            { 
                //CanNotHide();
                photonView.RPC("RPCCannotHide", RpcTarget.All);
            }
        }

    }


    [PunRPC]
    public void RPCMyColor()
    {
        // ID로 접근 --> 쉐이더 그래프 ID -> int값 출력
        Paintable paintable = hitInfo.transform.GetComponent<Paintable>();
        if (paintable != null)
        {
            RenderTexture render = paintable.getMask();
            if (render == null) print("render is null");
            Texture2D text = RenderTextureTo2DTexture(render);
            if (text == null) print("text is null");
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


    // 숨는 버튼을 누르고 내 색깔일 때 실행되는 함수 
    void CanHideP()
    {
        // 오징어로 변해도 공격당할 수 있게 함
        // 잉크 충전 UI킨다
        UI_chageInk.SetActive(true);
        UI_ChagerInkPaint.SetActive(true);
        VFX_squid_Speed.SetActive(true);

        //sphere.gameObject.SetActive(true);
        //// 플레이어 안보이게 함
        //body.gameObject.SetActive(false);
        //weapon.gameObject.SetActive(false);
        //inkTank.SetActive(false);

        // 만약 플레이어가 롤러라면
        if (gameObject.name.Contains("Roller"))
        {
            // 속도빠르게 하기
            Roller_Move rm = gameObject.GetComponent<Roller_Move>();
            rm.isRun = true;
            
            // 잉크 충전하기 
            PlayerRoller pr = GetComponent<PlayerRoller>();
            pr.ChargeInk();
            print("롤러 잉크 충전된다!");
        }
        else if (gameObject.name.Contains("Shooter"))
        {
            ShooterMovement pm = GetComponent<ShooterMovement>();
            pm.isRun = true;
            PlayerShooter ps = GetComponent<PlayerShooter>();
            // 총알 충전을 위한 함수
            ps.ChargeInk();
            print("슈터 잉크 충전된다!");
        }

        // 플레이어가 차저라면
        else if (gameObject.name.Contains("Charger"))
        {
            Charger_Move cm = GetComponent<Charger_Move>();
            cm.isRun = true;
            PlayerCharger pc = GetComponent<PlayerCharger>();
            // 총알 충전을 위한 함수
            pc.ChargeInk();
            print("차저 잉크 충전된다!");
        }

        // 숨을 때 바닥에 닫는다면
        // 오징어가 보이지 않게한다
        // 바닥에 닿았는지 확인하기 위한 RAY
        //Ray groundRay = new Ray(transform.position, transform.up * -1);
        //RaycastHit hitGround;
        //if (Physics.Raycast(groundRay, out hitGround))
        //{
        //    float distance = Vector3.Distance(transform.position, hitGround.point);
        //    // 바닥에 닿아있다면 
        //    // 아무것도 안보이게 한다
        //    // 충전을 위한 UI는 보이게한다.
        //    if (distance < 0.5f)
        //    {
        //        squid.SetActive(false);
        //    }
        //    else
        //    {
        //        squid.SetActive(true);
        //        //squid.transform.forward = transform.forward;

        //    }
        //    print("길이 : " + distance);
        //}

    }

    [PunRPC]
    public void RPCCanHide()
    {
        // 들어가고 나오고 움직이고 파티클 동기화
        if (!particle_Ink_Hiding.isPlaying)
        {
            particle_Ink_Hiding.Play();
        }
        if (Input.GetKeyDown(hideKey))
        {
            particle_Ink_Splash.Play();
        }

        // 오징어로 변해도 공격당할 수 있게 함
        sphere.gameObject.SetActive(true);

        // 플레이어 안보이게 함
        body.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        inkTank.SetActive(false);

        //// 만약 플레이어가 롤러라면
        //if (gameObject.name.Contains("Roller"))
        //{
        //    Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
        //    rMove.speed = 13;
        //}
        //else if (gameObject.name.Contains("Shooter"))
        //{
        //    PlayerMovement pm = GetComponent<PlayerMovement>();
        //    pm.run = true;
        //    PlayerShooter ps = GetComponent<PlayerShooter>();
        //    // 총알 충전을 위한 함수
        //    ps.ChargeInk();
        //    print("잉크 충전된다!");
        //}

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
        }
    }



    void CanNotHide()
    {
        sphere.gameObject.SetActive(false);
        body.gameObject.SetActive(true);
        weapon.gameObject.SetActive(true);
        inkTank.SetActive(true);

        // 만약 플레이어가 롤러라면
        if (gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 8;
        }
        squid.SetActive(false);
    }

    [PunRPC]
    public void RPCCannotHide()
    {
        // 파티클 멈추기 동기화

        particle_Ink_Hiding.Stop();

        sphere.gameObject.SetActive(false);
        body.gameObject.SetActive(true);
        weapon.gameObject.SetActive(true);
        inkTank.SetActive(true);

        // 만약 플레이어가 롤러일 때
        if (gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            // 적 페인트 안에 있을 때
            if (isInenemyColor == true)
            {
                rMove.isInEnemyInk = true;
            }
            else
            {
                rMove.isInEnemyInk = false;
            }

            // 이건 공통이므로
            rMove.isRun = false;
        }

        // 만약 플레이어가 슈터라면
        else if (gameObject.name.Contains("Shooter"))
        {
            ShooterMovement sm = gameObject.GetComponent<ShooterMovement>();
            // 적 페인트 안에 있을 때
            if (isInenemyColor == true)
            {
                sm.isInEnemyInk = true;
            }
            // 그냥 바닥에 있을 때
            else
            {
                sm.isInEnemyInk = false;
            }

            // 이건 공통이므로
            sm.isRun = false;
        }

        // 플레이어가 차저라면
        else if(gameObject.name.Contains("Charger"))
        {
            Charger_Move cm = gameObject.GetComponent<Charger_Move>();
            // 적 페인트 안에 있을 때
            if (isInenemyColor == true)
            {
                cm.isInEnemyInk = true;
            }
            // 그냥 바닥에 있을 때
            else
            {
                cm.isInEnemyInk = false;
            }

            // 이건 공통이므로
            cm.isRun = false;
        }
        squid.SetActive(false);
    }
}
