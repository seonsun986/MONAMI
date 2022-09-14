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
    // Ray�� �ٴ����� ���
    // �ش� Ray�� ���� Pixel�� ���Ѵ�.
    
    void Start()
    {
        
        sphere.SetActive(false);
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

            Renderer rend = hitInfo.transform.GetComponent<MeshRenderer>();
            // ID�� ���� --> ���̴� �׷��� ID -> int�� ���
            Texture2D text = rend.material.mainTexture as Texture2D;
            Vector2 pixelUV = hitInfo.textureCoord;
            pixelUV.x *= text.width;
            pixelUV.y *= text.height;
            print(text.GetPixel((int)pixelUV.x, (int)pixelUV.y));
            
            //    MyColor();
            //    Color color = t2D.GetPixel(100 , 100);
            //    print("�̾ƿ� ���� #" + ColorUtility.ToHtmlStringRGB(color));
            //    string getColor = ColorUtility.ToHtmlStringRGB(color);
            //    // �� ������ ��
            //    if (getColor.Substring(0,1)== "6" || getColor.Substring(0, 1) == "7")
            //    {
            //        canHide = true;
            //    }
            //    // ����� �����϶�
            //    else if(getColor.Substring(0, 1) == "F")
            //    {
            //        print("�� ���´�!");
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
            //    // ���� �� ���� ��
            //    CanHideP();
            //}
            //else
            //{
            //    // ���� �� ���� ��
            //    CanNotHide();
            //}
        }
    }

    void MyColor()
    {
        //t2D = TextureToTexture2d(rawImage.texture);
        //�ؽ��Ŀ��� �̴´�
        //var pixelData = t2D.GetPixels();
        //print("Total pixels " + pixelData.Length);
        //var colorIndex = new List<Color>();
        //var total = pixelData.Length;
        //for (var i = 0; i < total; i++)
        //{
        //    var color = pixelData[i];
        //    if (colorIndex.IndexOf(color) == -1)
        //    {
        //        colorIndex.Add(color);
        //    }
        //}
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
        // ��¡��� ���ص� ���ݴ��� �� �ְ� ��
        sphere.gameObject.SetActive(true);
        // �÷��̾� �Ⱥ��̰� ��
        body.gameObject.SetActive(false);
        weapon.gameObject.SetActive(false);

        // ���� �÷��̾ �ѷ����
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

        // ���� �÷��̾ �ѷ����
        if(gameObject.name.Contains("Roller"))
        {
            Roller_Move rMove = gameObject.GetComponent<Roller_Move>();
            rMove.speed = 8;
        }
    }
}
