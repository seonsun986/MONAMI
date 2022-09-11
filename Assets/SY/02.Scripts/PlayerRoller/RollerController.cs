using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController: MonoBehaviour{
    
    //������ ������
    public Color paintColor;
    //����
    public float radius = 1;
    //����
    public float strength = 1;
    //�浵...??
    public float hardness = 1;

    

    private void OnCollisionStay(Collision other)
    {
        Paintable p = other.collider.GetComponent<Paintable>();
        if (p != null)
        {
            //contacts = ���������� ���� �����Ǵ� ��ü ���� �浹����.
            //������ �������� �� �� ����-> �迭 Ÿ�� ��ȯ
            //contacts[0].point = ù��° �浹 ������ ��ġ
            //�� PaintManager�� �ִ� ����Ʈ�� �ε��� ��ġ�� ���������شٴ� ��!!
            Vector3 pos = other.contacts[0].point;
            PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
        }
    }
}