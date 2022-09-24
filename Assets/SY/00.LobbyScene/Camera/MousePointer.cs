using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePointer : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public static float2 GetScreenPosion(bool useOldSystem = true)
    {
        if(useOldSystem)
        {
            Vector3 posn = Input.mousePosition;
            return new float2(posn.x, posn.y);
        }
        else
        {
            return Mouse.current.position.ReadValue();
        }
    }
    public static float2 GetBoundedScreenPosition(bool useOldSystem = true)
    {
        float2 raw = GetScreenPosion(useOldSystem);
        return math.clamp(raw, new float2(0, 0), new float2(Screen.width - 1, Screen.height - 1));
    }

    public static Ray GetWorldRay(Camera camera, bool useOldSystem = true)
    {
        float2 screenPos = GetBoundedScreenPosition(useOldSystem);
        float3 screenPosWithDepth = new float3(screenPos, camera.nearClipPlane);
        return camera.ScreenPointToRay(screenPosWithDepth); 
    }
}
