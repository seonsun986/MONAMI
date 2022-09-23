using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller_Ink_Attack : MonoBehaviour
{

    //정해진 색깔을
    public Color paintColor;
    //범위
    public float radius = 1;
    //강도
    public float strength = 1;
    //경도
    public float hardness = 1;

    Rigidbody rb;
    public float inkSpeed;
    public float gravity = -20;
    float yVelocity = 0;

    Vector3 dir;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dir = transform.forward * 20 + transform.up * 15;
    }
    void Update()
    {
        rb.AddForce(dir.normalized * inkSpeed);
        
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