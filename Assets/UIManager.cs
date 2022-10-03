using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject Roller;
    public GameObject Charger;
    public GameObject Shooter;
    void Start()
    {
        // id 1~3 «Œ≈©∆¿
        if(DataManager.instance.id == 1)
        {
            if(DataManager.instance.weaponName == "Shooter")
            {
                GameObject shooter = Instantiate(Shooter, new Vector3(19866.7969f, 16196.2852f, 0), Quaternion.identity, GameObject.Find("Canvas").transform);
                shooter.GetComponent<Image>().color = new Color(1, 0, 0.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
