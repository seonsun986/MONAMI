using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoller : MonoBehaviour
{
    public GameObject leftRoller;
    public GameObject rightRoller;

    public bool mouseSingleClick;
    void Start()
    {
        leftRoller.SetActive(false);
        rightRoller.SetActive(false);
    }
    void Update()
    {
        bool click;
        click = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);

        if (click)
        {
            leftRoller.SetActive(true);
            rightRoller.SetActive(true);
        }
        else
        {
            leftRoller.SetActive(false);
            rightRoller.SetActive(false);
        }
    
    }
}
