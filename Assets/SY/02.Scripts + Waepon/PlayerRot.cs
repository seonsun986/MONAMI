using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//���콺�� �����ӿ� ����
//�¿� ȸ���� �÷��̾
//���� ȸ���� CamPos�� 
public class PlayerRot : MonoBehaviourPun
{
    //ȸ�� �ӷ�
    public float rotSpeed = 200;
    //CamPos�� Transform
    public Transform camPos;
    //ȸ���� ���� ����
    float rotX;
    float rotY;
    void Start()
    {
        
    }

    void Update()
    {
        //1. ���콺�� �������� �޴´�.
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        //2. ���콺�� �����Ӱ����� ȸ������ ������Ų��.
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        //3. �÷����� ȸ�� y���� �����Ѵ�.
        transform.localEulerAngles = new Vector3(0,rotX,0);
        //4. CamPos�� ȸ�� x���� �����Ѵ�.
        camPos.localEulerAngles = new Vector3(-rotY, 0, 0);
    }
}
