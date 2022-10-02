using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Charger_Ink : MonoBehaviourPun
{
    //정해진 색깔을
    public Color paintColor;
    //범위
    public float radius = 1;
    //강도
    public float strength = 1;
    //경도...??
    public float hardness = 1;

    public float inkSpeed;

    Rigidbody rb;
    public Camera cam;

    public GameObject hitImpactFactory;

    public int fillAmount;
    public string weaponName;

    void Start()
    {
        weaponName = DataManager.instance.weaponName;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * inkSpeed, ForceMode.Impulse);
    }

    RaycastHit hitForward;
    public float radiusByCharge;        // 차징 시간에 따른 잉크 크기
    void Update()
    {
        RaycastHit hitInfoF;
        Ray rayF = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayF, out hitInfoF))
        {
            hitForward = hitInfoF;
        }


        //레이를 바닥으로 쏘아 레이에 닿은 부분에 페인트 뿌리기
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


        if (gameObject.layer == LayerMask.NameToLayer("Player_Pink"))
        {
            Player_HP hp = other.gameObject.GetComponent<Player_HP>();
            if (hp != null)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player_Blue"))
                {
                    hp.hp += -(fillAmount);
                    print("hp " + -(fillAmount) + "줄인다!");
                }

                Destroy(gameObject);
                Destroy(hitImpact);

            }
        }


        if (gameObject.layer == LayerMask.NameToLayer("Player_Blue"))
        {
            Player_HP hp = other.gameObject.GetComponent<Player_HP>();
            if (hp != null)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player_Pink"))
                {
                    hp.hp += -(fillAmount);
                    print("hp " + -(fillAmount) + "줄인다!");
                }

                Destroy(gameObject);
                Destroy(hitImpact);

            }
        }
        
        //벽에 부딪혀 뿌리기

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
