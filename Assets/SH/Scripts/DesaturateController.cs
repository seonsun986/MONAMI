using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DesaturateController : MonoBehaviour
{

    [SerializeField] private ForwardRendererData rendererData = null;
    [SerializeField] private string featureName = null;
    [SerializeField] private float transitionPeriod = 1;

    public Material mat;
    private bool transitioning;
    private float startTime;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartTransition();
        //}
        //if (transitioning)
        //{
        //    if (Time.timeSinceLevelLoad >= startTime + transitionPeriod)
        //    {
        //        EndTransition();
        //    }
        //    else
        //    {
        //        UpdateTransition();
        //    }
        //}
        if (TryGetFeature(out var feature))
        {
            feature.SetActive(true);
            rendererData.SetDirty();

            var blitFeature = feature as BlitMaterialFeature;
            var material = mat;
        }
    }

    private void OnDestroy()
    {
        ResetTransition();
    }

    private bool TryGetFeature(out ScriptableRendererFeature feature)
    {
        feature = rendererData.rendererFeatures.Where((f) => f.name == featureName).FirstOrDefault();
        return feature != null;
    }

    private void StartTransition()
    {
        startTime = Time.timeSinceLevelLoad;
        transitioning = true;
    }

    private void UpdateTransition()
    {
        if (TryGetFeature(out var feature))
        {
            float saturation = Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / transitionPeriod);
            var blitFeature = feature as BlitMaterialFeature;
            var material = blitFeature.Material;
            material.SetFloat("_Saturation", saturation);
        }
    }

    private void EndTransition()
    {
        if (TryGetFeature(out var feature))
        {
            feature.SetActive(false);
            rendererData.SetDirty();

            transitioning = false;
        }
    }

    private void ResetTransition()
    {
        if (TryGetFeature(out var feature))
        {
            feature.SetActive(true);
            rendererData.SetDirty();

            var blitFeature = feature as BlitMaterialFeature;
            var material = blitFeature.Material;
            material.SetFloat("_Saturation", 0);

            transitioning = false;
        }
    }
}
