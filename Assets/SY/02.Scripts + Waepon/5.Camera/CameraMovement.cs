using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraMovement : MonoBehaviourPun
{
    //���󰡾� �� ������Ʈ�� ����
    public Transform objectTofollow;
    //���� ���ǵ�
    public float followSpeed = 10f;
    //���콺 ����
    public float sensitivity = 100f;
    //ī�޶� ���Ϸ� ������ �� ���� ����
    public float clampAngle = 70f;

    //���콺 ��ǲ�� ���� ����
    private float rotX;
    private float rotY;

    //ī�޶� ����
    public Transform realCamera;
    //���� : ũ��� ������ �� �� ������ ����.
    public Vector3 dirNomalized;
    //���������� ������ ������ �������� ���� ��
    public Vector3 finalDir;
    //�ּҰŸ�
    public float minDistance;
    //�ִ�Ÿ�
    public float maxDistance;
    //���������� ������ �Ÿ�
    public float finalDistace;
    //�ε巯���� ����
    public float smoothness = 10f;

    //ī�޶� ��
    public float zoomDistance;

    public GameObject cam;

    void Start()
    {
        if (photonView.IsMine == true)
        {
            cam.SetActive(true);
        }
        //ó�� ������ �� ������ �ʱ�ȭ
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        //���Ͱ��� �ʱ�ȭ, ��ֶ���� �ϸ� ũ�Ⱑ 0���� �Ǽ� ���⸸ ����
        dirNomalized = realCamera.localPosition.normalized;
        //magnitude : ũ��
        finalDistace = realCamera.localPosition.magnitude;

        //���Ӿ����� ���콺 �� �Ž�����
        /* Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;*/
    }

    void Update()
    {
        //�������Ӹ��� ��ǲ�� �ޱ� ����
        //X���� �������� ī�޶� ������ ���� ���콺�� ���Ϸ� �����̴� 
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        //rotX : ī�޶��� ���� ȸ���� ���� -70~70���� ���������ش�
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        //ī�޶��� ȸ���� rot��ŭ �������ش�
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    void LateUpdate()
    {

        //���󰡰�
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);
        //���ý����̽����� ���彺���̽��� �ٲ��� (���� x �ִ�Ÿ�);
        finalDir = transform.TransformPoint(dirNomalized * (maxDistance - zoomDistance));

        RaycastHit hit;
        Debug.DrawRay(transform.position, finalDir, Color.red);
        int layer = 1 << LayerMask.NameToLayer("Wall");
        if (Physics.Linecast(transform.position, finalDir, out hit, layer))
        {
            //���࿡ ������ �׷��� �� ���� ������ (���� ���� �Ÿ�->�ּҰŸ�)
            finalDistace = Mathf.Clamp(hit.distance, (minDistance - zoomDistance), (maxDistance - zoomDistance));
        }
        else
        {
            //���࿡ ���� ���ٸ� �׳� �ִ�Ÿ��� �ݿ�����.
            finalDistace = maxDistance - zoomDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNomalized * finalDistace, Time.deltaTime * smoothness);
    }
}
