using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
    public GameObject popup;
    public GameObject image;

    public GameObject shooter;
    public GameObject roller;
    public GameObject charger;

    public GameObject shooterInfo;
    public GameObject rollerInfo;
    public GameObject chargerInfo;

    public void Start()
    {
        image.SetActive(false);
    }

    void Update()
    {
        if (popup.activeSelf)
        {
            fuck(false);
            shooterInfo.SetActive(false);
            rollerInfo.SetActive(false);
            chargerInfo.SetActive(false);

        }
        else
            fuck(true);
        /*shooterInfo.SetActive(true);
        rollerInfo.SetActive(true);
        chargerInfo.SetActive(true);*/

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
