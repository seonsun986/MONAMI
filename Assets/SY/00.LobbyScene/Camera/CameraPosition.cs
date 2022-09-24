using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform target;

    public GameObject UI_Canvas;
    void Start()
    {
        //UIĵ���� ���ֱ�
        UI_Canvas.SetActive(false);
    }

    void Update()
    {
        //ī�޶� ������ ��ġ�� �̵� �����ֱ�
        transform.localPosition = Vector3.Lerp(transform.localPosition,target.localPosition, 0.02f);

        //����ī�޶��� ������ �ִ� ������ x���� 0 �̻��̸�
        if(transform.localPosition.x > 0 )
        {
            //UI ĵ���� ����.
            UI_Canvas.SetActive(true);
        }
    }
}
