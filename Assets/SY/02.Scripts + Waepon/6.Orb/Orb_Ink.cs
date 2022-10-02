using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ε��� ���� ������, 1.5�ʶ�� �ð����� ������ �ڱ� �� ���� �� �� �ְ� ������ش�.
//��������� ���̸� �޾��� ����, ���̰� ���� ��ġ�� ����Ʈ�� �׷��ּ���.

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
        //���̸� �ٴ����� ��� ���̿� ���� �κп� ����Ʈ �Ѹ���
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
