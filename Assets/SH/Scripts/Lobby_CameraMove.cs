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
        // ������ �� ����ڰ� ������ ���� ������ �����ϱ�
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

        // ���� ���� �α�
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
