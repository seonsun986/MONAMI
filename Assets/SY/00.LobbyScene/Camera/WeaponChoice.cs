using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
    public GameObject popup;

    public GameObject shooter;
    public GameObject roller;
    public GameObject charger;


    void Update()
    {
        if (popup.activeSelf)
        {
            fuck(false);
        }
        else
            fuck(true);
        
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
                }
                if (hit.transform.gameObject.tag == "Roller")
                {
                    //�ѷ��� �����Ͽ���.
                    print("�ѷ��� �����Ͽ���.");
                    popup.SetActive(true);
                    DataManager.instance.weaponName = "Roller";
                }
                if (hit.transform.gameObject.tag == "Charger")
                {
                    //������ �����Ͽ���.
                    print("������ �����Ͽ���.");
                    popup.SetActive(true);
                    DataManager.instance.weaponName = "Charger";
                }
                
            }

        }
        void fuck(bool B)
        {
            shooter.GetComponent<SphereCollider>().enabled = B;
            roller.GetComponent<SphereCollider>().enabled = B;
            charger.GetComponent<SphereCollider>().enabled = B;
        }
    }
}
