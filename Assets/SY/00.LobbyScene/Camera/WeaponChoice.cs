using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
    public GameObject popup;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                //���� ī�޶� �ε��� ���� �±װ� ���Ͷ��
                if (hit.transform.gameObject.tag == "Shooter")
                {
                    //���͸� �����Ͽ���.
                    print("���͸� �����Ͽ���.");
                    popup.SetActive(true);
                    DataManager.instance.weaponName = "Shooter";
                    //������ �Ѱ�����?

                }
                else if(hit.transform.gameObject.tag == "Roller")
                {
                    //�ѷ��� �����Ͽ���.
                    print("�ѷ��� �����Ͽ���.");
                    popup.SetActive(true);
                    DataManager.instance.weaponName = "Roller";
                }
                else
                {
                    //������ �����Ͽ���.
                    print("������ �����Ͽ���.");
                    popup.SetActive(true);
                    DataManager.instance.weaponName = "Charger";
                }
            }

        }
    }
}
