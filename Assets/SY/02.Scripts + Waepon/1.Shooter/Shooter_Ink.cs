using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


//잉크의 방향 , 중력, Addforce
public class Shooter_Ink : MonoBehaviourPun
{
    //정해진 색깔을
    public Color paintColor;
    //범위
    public float radius = 1;
    //강도
    public float strength = 1;
    //경도
    public float hardness = 1;


    void Start()
    {
    }

    void Update()
    {


    }


    private void OnCollisionEnter(Collision other)
    {
        // 총알이 핑크라면
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

        // 총알이 블루라면
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
            //contacts = 물리엔진에 의해 생성되는 물체 간의 충돌지점.
            //접점은 여러개가 될 수 있음-> 배열 타입 반환
            //contacts[0].point = 첫벗째 충돌 지점의 위치
            //즉 PaintManager에 있는 페인트를 부딪힌 위치에 생성시켜준다는 뜻!!
            Vector3 pos = other.contacts[0].point;
            PaintManager.instance.photonView.RPC("RPCPaint", Photon.Pun.RpcTarget.All, p.id, pos, radius, hardness, strength, paintColor.r, paintColor.g, paintColor.b);
            //PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);

            Destroy(gameObject);
        }

    }
}
