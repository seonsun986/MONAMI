using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb_Line : MonoBehaviour
{
    public GameObject cam;
    public GameObject orb;

    public Transform firePos;
    public LineRenderer lr;

    public float gravity = -9.81f;
    public float jumpPower = 10;
    float curveDeltaTime = 1 / 60f;

    public int maxPoint = 1000;
    int pointCount = 0;

    //������ ������
    Vector3 pos;

    //���콺 ��ǲ�� ���� ����
    private float rotX;
    float mouseValue;
    //���콺 ����
    public float sensitivity = 100f;

    void Start()
    {
        //ó�� ������ �� ������ �ʱ�ȭ
        rotX = transform.localRotation.eulerAngles.x;
    }

    Vector3 velocity;
    Vector3 firstVelocity;

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            mouseValue++;
        }
        //�������Ӹ��� ��ǲ�� �ޱ� ����
        //X���� �������� ī�޶� ������ ���� ���콺�� ���Ϸ� �����̴� 
        rotX = mouseValue;
        //rotX : ī�޶��� ���� ȸ���� ���� -70~70���� ���������ش�
        //ī�޶��� ȸ���� rot��ŭ �������ش�
        Quaternion rot = Quaternion.Euler(rotX, 0, 0);
        transform.rotation = rot;

        //curveDeltaTime = 1 / (float)maxPoint;
        pointCount = 0;
        firstVelocity = velocity = firePos.forward * jumpPower;
        pos = firePos.transform.position;
        lr.positionCount = 2;

        RaycastHit hitInfo;

        if(Input.GetMouseButton(0))
        {
            cam.GetComponentInParent<CameraMovement>().zoomDistance = 3f;
            //����Ʈī��Ʈ�� �ƽ� ����Ʈ���� ���� ���� ����.
            for (int i = 0; i < maxPoint; i++)
            {
                if (false == MakeCurve(out hitInfo))
                {
                    //========================================���⼭ ���ϴ� ���� ����
                    // ���� ���� �ִ�.
                    // �������� ��ƼŬ ��� �������� ����.
                    

                    //makeCurve�� false�� for���� ����!
                    break;
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            cam.GetComponentInParent<CameraMovement>().zoomDistance = 0f;
            GameObject go = Instantiate(orb);
            go.transform.position = pos;
            go.GetComponent<Rigidbody>().velocity = firstVelocity;
        }

    }
    bool MakeCurve(out RaycastHit hitInfo)
    {

        //ù��° ���� ��(0)�� ���̸� ���� �ʰڴ�. (����ó��)
        if (pointCount == 0)
        {
            lr.positionCount = pointCount + 1;
            lr.SetPosition(pointCount, pos);
            ++pointCount;
            hitInfo = new RaycastHit();
            hitInfo.point = pos;
            return true;
        }

        //�߷��� �ݿ�
        //gravity : ����
        velocity += gravity * Vector3.up * curveDeltaTime;
        //���� ����� ���ǵ�
        pos += velocity * curveDeltaTime;

        //���� ��ġ���� ���ο� ��ġ�� Ray�� ���ʹ�.
        Vector3 prevPos = lr.GetPosition(pointCount - 1);
        Vector3 dir = pos - prevPos;
        Ray ray = new Ray(prevPos, dir);
        //�Ÿ� �־��ֱ�, ���̸� ������ �������� ���� �� ���� 
        if (Physics.Raycast(ray, out hitInfo, dir.magnitude))
        {
            lr.positionCount = pointCount + 1;
            lr.SetPosition(pointCount, hitInfo.point);
            ++pointCount;
            //�ε����� ������ �׸���!!
            return false;
        }
        else
        {
            lr.positionCount = pointCount + 1;
            lr.SetPosition(pointCount, pos);
            ++pointCount;
            //�����!!
            return true;
        }

    }
}
