using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger_Ink : MonoBehaviour
{
    //������ ������
    public Color paintColor;
    //����
    public float radius = 1;
    //����
    public float strength = 1;
    //�浵...??
    public float hardness = 1;

    public float inkSpeed;

    Rigidbody rb;
    public Camera cam;

    public GameObject hitImpactFactory;
    Vector3 center;
    void Start()
    {
        //Vector3 center = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2);
        //Vector3 wcenter = cam.ScreenToWorldPoint(center);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * inkSpeed, ForceMode.Impulse);
    }

    RaycastHit hitForward;
    void Update()
    {

        RaycastHit hitInfoF;
        Ray rayF = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayF, out hitInfoF))
        {
            hitForward = hitInfoF;
        }


        //���̸� �ٴ����� ��� ���̿� ���� �κп� ����Ʈ �Ѹ���
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Paintable p = hitInfo.collider.GetComponent<Paintable>();
            if (p != null)
            {
                PaintManager.instance.paints(p, hitInfo.point, radius, hardness, strength, paintColor);
            }
        }

    }

   public float strength2 = 2;
    void OnCollisionEnter(Collision other)
    {

        GameObject hitImpact = Instantiate(hitImpactFactory);
        hitImpact.transform.position = hitForward.point;
        hitImpact.transform.forward = hitForward.normal;
        //����(��ũ)�� �ε��� ���� ���� �ƴ� ���� �� ���� �̶��!
        //if (other.collider.CompareTag("!isMine"))
        //{
        //    //�������� �ְ�ʹ�.

        //    Destroy(this.gameObject);
        //}
        //���� �ε��� �Ѹ���

        Paintable p = other.collider.GetComponent<Paintable>();
        if (p != null)
        {
            
            Vector3 pos = other.contacts[0].point;
            //PaintManager.instance.photonView.RPC("RPCPaint", Photon.Pun.RpcTarget.All, p.id, pos, radius, hardness, strength, paintColor.r, paintColor.g, paintColor.b);
            PaintManager.instance.paints(p, pos, radius, hardness, strength2, paintColor);

            Destroy(this.gameObject);
        }

    }
}
