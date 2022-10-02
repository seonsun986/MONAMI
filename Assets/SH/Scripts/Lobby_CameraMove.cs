using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby_CameraMove : MonoBehaviour
{
    float mx;
    float my;
    public float rotSpeed = 200;
    void Start()
    {
        // 시작할 때 사용자가 정해준 각도 값으로 시작하기
        mx = transform.eulerAngles.y;
        my = -transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;

        // 각도 제한 두기
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
