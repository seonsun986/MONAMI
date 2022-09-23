using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class localCanHide : MonoBehaviourPun
{
    public Camera cam;
    [Header("���� Ű")]
    [SerializeField]
    private KeyCode hideKey = KeyCode.LeftShift;
    public bool canHide;

    public Color[] colors;
    public string[] texts;

    public float rayDistance = 20;
    public GameObject sphere;       // ��¡��� ������ �� ���ݴ��ϱ� ����
    public GameObject body;
    public GameObject weapon;
    public GameObject inkTank;
    public GameObject UI_chageInk;          // ��ũ ���� UI
    public GameObject UI_ChagerInkPaint;    // ��ũ �޹�� UI(�����ִ�)
    Color playerColor;
    float myColor_R;
    float myColor_G;
    float myColor_B;
    public float[] rgb = new float[3];

    // ��¡�� ��Ÿ���� ���� �͵�
    CharacterController cc;
    public GameObject squid;

    //���� �� ����Ǵ� ��ƼŬ
    [SerializeField] ParticleSystem particle_Ink_Splash;
    //�������� �� ����Ǵ� ��ƼŬ
    [SerializeField] ParticleSystem particle_Ink_Hiding;
    //��¡�������� ������ ��
    [SerializeField] GameObject VFX_squid_Speed;

    // Ray�� �ٴ����� ���
    // �ش� Ray�� ���� Pixel�� ���Ѵ�.

    // ���� �� �Ѿ� count 0���� �ǻ츮�� ���� ��
    // ������ �ϴ� ���� UI�� ������ ���;��Ѵ�
    // �� �� ���� ��(canShoot == false)
    // 1�ʿ� 30���� �Ѿ� �����ǵ��� �Ѵ�
    // maxcount���� count�� �������� maxCount�� �ٽ� �ϰ� �Ѵ�
    // �ʿ�Ӽ� : canShoot, ��������, maxCount��
    void Start()
    {
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
    string colorName;
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hitInfo;

        // hideKey�� ������ ���� ������ �Ѵ�.
        if (Input.GetKey(hideKey) && Physics.Raycast(ray, out hitInfo))
        {
            //Zoom In
            cam.GetComponentInParent<Local_CameraMovement>().zoomDistance = 4f;
            // ID�� ���� --> ���̴� �׷��� ID -> int�� ���
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
                print("�̾ƿ� ���� #" + ColorUtility.ToHtmlStringRGB(color));
                print("RGB ���� : " + color.r + ", " + color.g + ", " + color.b);

                string getColor = ColorUtility.ToHtmlStringRGB(color);
                rgb[0] = color.r;
                rgb[1] = color.g;
                rgb[2] = color.b;


            }

            // ���������� ���� �� �� ������ ��
            if (rgb[0] < myColor_R + 0.4f && rgb[0] > myColor_R - 0.4f &&
                rgb[1] < myColor_G + 0.4f && rgb[1] > myColor_G - 0.4f &&
                rgb[2] > myColor_B - 0.4f && rgb[2] < myColor_B + 0.4f)
            {
                canHide = true;

            }
            // ���������� ���� �� ����� �����϶�
            else
            {
                canHide = false;
            }
        }
        if (Input.GetKeyUp(hideKey))
        {
            //Zoom Out
            cam.GetComponentInParent<Local_CameraMovement>().zoomDistance = 0f;
            if (canHide == true)
            { particle_Ink_Splash.Play(); }

            canHide = false;
            UI_chageInk.SetActive(false);
            UI_ChagerInkPaint.SetActive(false);
            VFX_squid_Speed.SetActive(false);
        }

        if (canHide == true)
        {

            // ���� �� ���� ��
            CanHideP();

        }
        else
        {
            // ���� �� ���� ��
            CanNotHide();

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


    // ���� ��ư�� ������ �� ������ �� ����Ǵ� �Լ� 
    void CanHideP()
    {
        if (!particle_Ink_Hiding.isPlaying)
        {
            particle_Ink_Hiding.Play();
        }
        if (Input.GetKeyDown(hideKey))
        {
            particle_Ink_Splash.Play();
        }

        // ��¡��� ���ص� ���ݴ��� �� �ְ� ��
        // ��ũ ���� UIŲ��
        UI_chageInk.SetActive(true);
        UI_ChagerInkPaint.SetActive(true);
        VFX_squid_Speed.SetActive(true);

        sphere.gameObject.SetActive(true);
        // �÷��̾� �Ⱥ��̰� ��
        body.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        inkTank.SetActive(false);

        // ���� �÷��̾ �ѷ����
        if (gameObject.name.Contains("Roller"))
        {
            Roller_Move rm = gameObject.GetComponent<Roller_Move>();
            rm.run = true;
            PlayerRoller pr = GetComponent<PlayerRoller>();
            pr.ChargeInk();
            print("�ѷ� ��ũ �����ȴ�!");

        }

        // �÷��̾ ���Ͷ��
        else if (gameObject.name.Contains("Shooter"))
        {
            ShooterMovement pm = GetComponent<ShooterMovement>();
            pm.run = true;
            PlayerShooter ps = GetComponent<PlayerShooter>();
            // �Ѿ� ������ ���� �Լ�
            ps.ChargeInk();
            print("���� ��ũ �����ȴ�!");
        }

        // �÷��̾ �������
        else if (gameObject.name.Contains("Charger"))
        {
            Charger_Move cm = GetComponent<Charger_Move>();
            cm.isRun = true;
            PlayerCharger pc = GetComponent<PlayerCharger>();
            // �Ѿ� ������ ���� �Լ�
            pc.ChargeInk();
            print("���� ��ũ �����ȴ�!");
        }




        // ���� �� �ٴڿ� �ݴ´ٸ�
        // ��¡� ������ �ʰ��Ѵ�
        // �ٴڿ� ��Ҵ��� Ȯ���ϱ� ���� RAY
        Ray groundRay = new Ray(transform.position, transform.up * -1);
        RaycastHit hitGround;
        if (Physics.Raycast(groundRay, out hitGround))
        {
            float distance = Vector3.Distance(transform.position, hitGround.point);
            // �ٴڿ� ����ִٸ� 
            // �ƹ��͵� �Ⱥ��̰� �Ѵ�
            // ������ ���� UI�� ���̰��Ѵ�.
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
        particle_Ink_Hiding.Stop();

        sphere.gameObject.SetActive(false);
        body.gameObject.SetActive(true);
        weapon.gameObject.SetActive(true);
        inkTank.SetActive(true);

        // ���� �÷��̾ �ѷ����
        if (gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 8;
        }
        squid.SetActive(false);
    }


}