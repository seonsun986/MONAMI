using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMouseOver : MonoBehaviour
{
    public GameObject info;

    private new Collider collider;
    private new MeshRenderer renderer;
    private bool lastOver;
    
    private void Start()
    {
        info.SetActive(false);
        collider = GetComponent<Collider>();

        SetOver(false);
    }

    private void Update()
    {
        bool isOver = Physics.Raycast(MousePointer.GetWorldRay(Camera.main), out var hit) && hit.collider == collider;
        if(isOver != lastOver)
        {
            SetOver(isOver);
        }

    }

    private void SetOver(bool isOver)
    {
        lastOver = isOver;
        info.SetActive(isOver);
    }
}
