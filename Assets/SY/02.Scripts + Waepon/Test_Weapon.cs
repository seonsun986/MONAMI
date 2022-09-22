using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Weapon : MonoBehaviour
{
    float weaponDamage = 1f;
    void Start()
    {

    }

    void Update()
    {

    }
  
    void OnTriggerEnter(Collider other)
    {
        //����) ���� ��� ���ӿ�����Ʈ�� Player��� �±׸� �ϰ� �ִٸ�
        if (other.gameObject.CompareTag("Player"))
        {
            print("�ε�����!!");
            //�ε��� ����� PlayerHP�� ������
            PlayerHP php = other.transform.GetComponent<PlayerHP>();
            if (php != null)
            {
                //PlayerHP�� �ִ� OnDamage �Լ� ����
                php.OnDamaged(weaponDamage);
            }
        }
    }
}
