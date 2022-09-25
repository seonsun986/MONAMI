using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using Photon.Pun;
using System.Collections.Generic;

public class PaintManager : Singleton<PaintManager>
{

    public Dictionary<int, Paintable> paint = new Dictionary<int, Paintable>();


    public Shader texturePaint;
    public Shader extendIslands;
    // �ٴ� 
    public GameObject plane;

    int prepareUVID = Shader.PropertyToID("_PrepareUV");
    int positionID = Shader.PropertyToID("_PainterPosition");
    int hardnessID = Shader.PropertyToID("_Hardness");
    int strengthID = Shader.PropertyToID("_Strength");
    int radiusID = Shader.PropertyToID("_Radius");
    int blendOpID = Shader.PropertyToID("_BlendOp");
    int colorID = Shader.PropertyToID("_PainterColor");
    int textureID = Shader.PropertyToID("_MainTex");
    int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    int uvIslandsID = Shader.PropertyToID("_UVIslands");

    Material paintMaterial;
    Material extendMaterial;

    CommandBuffer command;

    public override void Awake()
    {
        //��¡�� ���콺 Ŀ�� �Ⱥ��̰�
        Cursor.visible = false;
        base.Awake();

        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);
        command = new CommandBuffer();
        command.name = "CommmandBuffer - " + gameObject.name;
    }

    public void initTextures(Paintable paintable)
    {
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        command.SetRenderTarget(mask);
        command.SetRenderTarget(extend);
        command.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID, 1);
        command.SetRenderTarget(uvIslands);
        command.DrawRenderer(rend, paintMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    //[PunRPC]
    //public void RPCinitTxtures(Paintable paintable)
    //{
    //    RenderTexture mask = paintable.getMask();
    //    RenderTexture uvIslands = paintable.getUVIslands();
    //    RenderTexture extend = paintable.getExtend();
    //    RenderTexture support = paintable.getSupport();
    //    Renderer rend = paintable.getRenderer();

    //    command.SetRenderTarget(mask);
    //    command.SetRenderTarget(extend);
    //    command.SetRenderTarget(support);

    //    paintMaterial.SetFloat(prepareUVID, 1);
    //    command.SetRenderTarget(uvIslands);
    //    command.DrawRenderer(rend, paintMaterial, 0);

    //    Graphics.ExecuteCommandBuffer(command);
    //    command.Clear();
    //}

    public void paints(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null)
    {
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetTexture(textureID, support);
        paintMaterial.SetColor(colorID, color ?? Color.red);
        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
        extendMaterial.SetTexture(uvIslandsID, uvIslands);

        command.SetRenderTarget(mask);
        command.DrawRenderer(rend, paintMaterial, 0);

        command.SetRenderTarget(support);
        command.Blit(mask, support);

        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    [PunRPC]
    public void RPCPaint(int id, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, float r = 0, float g = 0, float b = 0)
    {
        Paintable paintable = paint[id];
        Color color = new Color(r, g, b, 1);
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetTexture(textureID, support);
        paintMaterial.SetColor(colorID, color);
        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
        extendMaterial.SetTexture(uvIslandsID, uvIslands);

        command.SetRenderTarget(mask);
        command.DrawRenderer(rend, paintMaterial, 0);

        command.SetRenderTarget(support);
        command.Blit(mask, support);

        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }
    public void AddPaint(Paintable p)
    {
        p.id = paint.Count;
        paint.Add(p.id, p);
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("��ư�� �������ϴ�.");
            Paintable paint = plane.GetComponent<Paintable>();

            RenderTexture resultTexture = paint.getMask();

            SaveRenderTextureToPNG(resultTexture, "Assets/Python/images/", "result");
        }
    }

}
