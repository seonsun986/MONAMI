using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//내가 선택무기, 내 ID
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string weaponName;
    public int id;

    void Start()
    {

    }

    void Update()
    {

    }
}
