using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform target;

    public GameObject UI_Canvas;
    void Start()
    {
        //UI캔버스 꺼주기
        UI_Canvas.SetActive(false);
    }

    void Update()
    {
        //카메라를 목적지 위치로 이동 시켜주기
        transform.localPosition = Vector3.Lerp(transform.localPosition,target.localPosition, 0.02f);

        //메인카메라이 가지고 있는 포지션 x값이 0 이상이면
        if(transform.localPosition.x > 0 )
        {
            //UI 캔버스 켜줌.
            UI_Canvas.SetActive(true);
        }
    }
}
