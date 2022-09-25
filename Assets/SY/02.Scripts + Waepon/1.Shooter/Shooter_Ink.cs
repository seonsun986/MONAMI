using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


//��ũ�� ���� , �߷�, Addforce
public class Shooter_Ink : MonoBehaviourPun
{
    //������ ������
    public Color paintColor;
    //����
    public float radius = 1;
    //����
    public float strength = 1;
    //�浵
    public float hardness = 1;


    void Start()
    {
    }

    void Update()
    {


    }


    private void OnCollisionEnter(Collision other)
    {
        // �Ѿ��� ��ũ���
        if (gameObject.layer == LayerMask.NameToLayer("Player_Pink"))
        {
            Player_HP hp = other.gameObject.GetComponent<Player_HP>();
            if (hp != null)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Player_Blue"))
                {
                    hp.hp--;
                }

                Destroy(gameObject);

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
                    hp.hp--;
                }

                Destroy(gameObject);

            }
        }

        Paintable p = other.collider.GetComponent<Paintable>();
        if (p != null)
        {
            //contacts = ���������� ���� �����Ǵ� ��ü ���� �浹����.
            //������ �������� �� �� ����-> �迭 Ÿ�� ��ȯ
            //contacts[0].point = ù��° �浹 ������ ��ġ
            //�� PaintManager�� �ִ� ����Ʈ�� �ε��� ��ġ�� ���������شٴ� ��!!
            Vector3 pos = other.contacts[0].point;
            PaintManager.instance.photonView.RPC("RPCPaint", Photon.Pun.RpcTarget.All, p.id, pos, radius, hardness, strength, paintColor.r, paintColor.g, paintColor.b);
            //PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);

            Destroy(gameObject);
        }

    }
}
