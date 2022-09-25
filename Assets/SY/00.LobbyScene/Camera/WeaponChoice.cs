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
                //만약 카메라에 부딪힌 것의 태그가 슈터라면
                if (hit.transform.gameObject.tag == "Shooter")
                {
                    //슈터를 선택하였다.
                    print("슈터를 선택하였다.");
                    popup.SetActive(true);
                    DataManager.instance.weaponName = "Shooter";
                }
                if (hit.transform.gameObject.tag == "Roller")
                {
                    //롤러를 선택하였다.
                    print("롤러를 선택하였다.");
                    popup.SetActive(true);
                    DataManager.instance.weaponName = "Roller";
                }
                if (hit.transform.gameObject.tag == "Charger")
                {
                    //차저를 선택하였다.
                    print("차저를 선택하였다.");
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
