using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller_Ink_Attack : MonoBehaviour
{

    //������ ������
    public Color paintColor;
    //����
    public float radius = 1;
    //����
    public float strength = 1;
    //�浵...??
    public float hardness = 1;

    Rigidbody rb;
    public float inkSpeed;
    public GameObject vector;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 vector = transform.forward + new Vector3(0, 5, 10);
        rb.AddForce(vector.normalized * inkSpeed);
    }

    private void OnCollisionStay(Collision other)
    {
        Paintable p = other.collider.GetComponent<Paintable>();
        if (p != null)
        {

            Vector3 pos = other.contacts[0].point;
            //PaintManager.instance.photonView.RPC("RPCPaint", Photon.Pun.RpcTarget.All, p.id, pos, radius, hardness, strength, paintColor.r, paintColor.g, paintColor.b);

            PaintManager.instance.paints(p, pos, radius, hardness, strength, paintColor);

            Destroy(this.gameObject);
        }
    }
}