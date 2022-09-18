using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanHide : MonoBehaviour
{
    [Header("���� Ű")]
    [SerializeField]
    private KeyCode hideKey = KeyCode.LeftShift;
    [SerializeField]
    private bool canHide;

    public Color[] colors;
    public string[] texts;

    public Texture2D imageMap;

    public float rayDistance = 20;
    public RawImage rawImage;
    public GameObject sphere;       // ��¡��� ������ �� ���ݴ��ϱ� ����
    public GameObject body;
    public GameObject weapon;
    public GameObject inkTank;
    public GameObject UI_chageInk;
    public float[] rgb = new float[3];

    // ��¡�� ��Ÿ���� ���� �͵�
    CharacterController cc;
    public GameObject squid;
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

            // ID�� ���� --> ���̴� �׷��� ID -> int�� ���
            Paintable paintable = hitInfo.transform.GetComponent<Paintable>();
            if (paintable != null)
            {
                RenderTexture render = paintable.getMask();
                Texture2D text = RenderTextureTo2DTexture(render);
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

            // �� ������ ��
            if (rgb[0] < 1.1f && rgb[0] > 0.9f && rgb[1] == 0 && rgb[2] > 0.4f && rgb[2] < 0.6f)
            {
                canHide = true;
            }
            // ����� �����϶�
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
            // ���� �� ���� ��
            CanHideP();
        }
        else
        {
            // ���� �� ���� ��
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
        // ��¡��� ���ص� ���ݴ��� �� �ְ� ��
        // ��ũ ���� UIŲ��
        UI_chageInk.SetActive(true);
        sphere.gameObject.SetActive(true);
        // �÷��̾� �Ⱥ��̰� ��
        body.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);
        inkTank.SetActive(false);

        // ���� �÷��̾ �ѷ����
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
            // �Ѿ� ������ ���� �Լ�
            ps.ChargeInk();
            print("��ũ �����ȴ�!");
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
            print("���� : " + distance);
        }

    }

    
    void CanNotHide()
    {
        sphere.gameObject.SetActive(false);
        body.gameObject.SetActive(true);
        weapon.gameObject.SetActive(true);
        inkTank.SetActive(true);

        // ���� �÷��̾ �ѷ����
        if(gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 8;
        }
        squid.SetActive(false);
    }
}
