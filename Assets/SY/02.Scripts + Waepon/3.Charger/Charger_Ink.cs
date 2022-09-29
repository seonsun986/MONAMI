using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Charger_Ink : MonoBehaviourPun
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
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * inkSpeed, ForceMode.Impulse);
    }

    RaycastHit hitForward;
    public float radiusByCharge;        // ��¡ �ð��� ���� ��ũ ũ��
    void Update()
    {
        RaycastHit hitInfoF;
        Ray rayF = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayF, out hitInfoF))
        {
            hitForward = hitInfoF;
        }


        //���̸� �ٴ����� ��� ���̿� ���� �κп� ����Ʈ �Ѹ���
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Paintable p = hitInfo.collider.GetComponent<Paintable>();
            if (p != null)
            {
                Vector3 pos = hitInfo.point;
                PaintManager.instance.photonView.RPC("RPCPaint", Photon.Pun.RpcTarget.All, p.id, pos, radiusByCharge, hardness, strength, paintColor.r, paintColor.g, paintColor.b);

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

        Player_HP hp = other.gameObject.GetComponent<Player_HP>();
        if(hp!=null)
        {
            print(gameObject.name);
            if(gameObject.layer == LayerMask.NameToLayer("Player_Pink"))
            {
                if(other.gameObject.layer == LayerMask.NameToLayer("Player_Blue"))
                {
                    hp.hp -= 3;
                    print("hp 3���δ�");
                }
            }

            else if(gameObject.layer == LayerMask.NameToLayer("Player_Blue"))
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player_Pink"))
                {
                    hp.hp -= 3;
                    print("hp 3���δ�");

                }
            }
        }     
         Destroy(gameObject);         

        //���� �ε��� �Ѹ���

        Paintable p = other.collider.GetComponent<Paintable>();
        if (p != null)
        {
            
            Vector3 pos = other.contacts[0].point;
            PaintManager.instance.photonView.RPC("RPCPaint", Photon.Pun.RpcTarget.All, p.id, pos, radius, hardness, strength, paintColor.r, paintColor.g, paintColor.b);

            Destroy(this.gameObject);
            Destroy(hitImpact);
        }

    }
}
