using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastMouseOver : MonoBehaviour
{
    public AudioSource clickSound;

    public GameObject info;
    public GameObject impact;

    private new Collider collider;
    private bool lastOver;

    public GameObject popup;

    int count;
    private void Start()
    {
        impact.SetActive(false);
        info.SetActive(false);
        collider = GetComponent<Collider>();

        SetOver(false);
    }

    private void Update()
    {
        bool isOver = Physics.Raycast(MousePointer.GetWorldRay(Camera.main), out var hit) && hit.collider == collider;
        if (isOver != lastOver)
        {
            SetOver(isOver);
        }
        


    }

    private void SetOver(bool isOver)
    {
        if (popup.activeSelf == false)
        {
            lastOver = isOver;
            info.SetActive(isOver);
            impact.SetActive(isOver);
            if (clickSound.isPlaying) return;
            clickSound.Play();
        }
    }
}
