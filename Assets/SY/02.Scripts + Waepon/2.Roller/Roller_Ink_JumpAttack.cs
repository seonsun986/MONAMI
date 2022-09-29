using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller_Ink_JumpAttack : MonoBehaviour
{

    //������ ������
    public Color paintColor;
    //����
    public float radius = 1;
    //����
    public float strength = 1;
    //�浵
    public float hardness = 1;

    Rigidbody rb;
    public string weaponName;


    Vector3 dir;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dir = (transform.forward + transform.up).normalized * 1;

        rb.velocity = dir * 5f;
        weaponName = DataManager.instance.weaponName;

    }
    void Update()
    {
        rb.velocity += Vector3.down * 0.3f;

    }

    private void OnCollisionStay(Collision other)
    {

        // �Ѿ��� ��ũ���
        if (gameObject.layer == LayerMask.NameToLayer("Player_Pink"))
        {
            Player_HP hp = other.gameObject.GetComponent<Player_HP>();
            if (hp != null)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player_Blue"))
                {
                    hp.weaponName = weaponName;
                    hp.hp--;
                }
            }

        }

        // �Ѿ��� �����
        if (gameObject.layer == LayerMask.NameToLayer("Player_Blue"))
        {
            Player_HP hp = other.gameObject.GetComponent<Player_HP>();
            if (hp != null)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player_Pink"))
                {
                    hp.weaponName = weaponName;
                    hp.hp--;
                }
            }
        }

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