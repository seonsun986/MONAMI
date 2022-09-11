using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoller : MonoBehaviour
{
    public GameObject roller;

    public bool mouseSingleClick;
    void Start()
    {
        roller.SetActive(false);
    }
    void Update()
    {
        bool click;
        click = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);

        if (click)
        {
            roller.SetActive(true);
        }
        else
        {
            roller.SetActive(false);
        }
    
    }
}
