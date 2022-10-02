using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//부딪힌 순간 켜지고, 1.5초라는 시간동안 빠르게 자기 할 일을 할 수 있게 만들어준다.
//양방향으로 레이를 달아준 다음, 레이가 닿은 위치에 페인트를 그려주세요.

public class Orb_Ink : MonoBehaviour
{
    [Header("Ink")]
    [SerializeField] Color paintColor;
    [SerializeField] float radius = 0;
    [SerializeField] float strength = 0;
    [SerializeField] float hardness = 0;

    void Start()
    {
    }

    void Update()
    {
        //=======Grond=======
        //레이를 바닥으로 쏘아 레이에 닿은 부분에 페인트 뿌리기
        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hitInfo;
            Debug.DrawRay(transform.position, transform.up * -1*50f,Color.red);
        if (Physics.Raycast(ray, out hitInfo,50f,~LayerMask.NameToLayer("Wall")))
        {

            Paintable p = hitInfo.collider.GetComponent<Paintable>();
            if (p != null)
            {
                Vector3 pos = hitInfo.point;
                //PaintManager.instance.photonView.RPC("RPCPaint", Photon.Pun.RpcTarget.All, p.id, pos, radiusByCharge, hardness, strength, paintColor.r, paintColor.g, paintColor.b);
                PaintManager.instance.paints(p, pos, radius, hardness, strength, paintColor);
            }
        }
       
    }

}
