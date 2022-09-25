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
    // 바닥 
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
        //오징어 마우스 커서 안보이게
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("버튼을 눌렀습니다.");
            Paintable paint = plane.GetComponent<Paintable>();

            RenderTexture resultTexture = paint.getMask();

            SaveRenderTextureToPNG(resultTexture, "Assets/Python/images/", "result");
        }
    }

}
