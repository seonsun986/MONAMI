using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���ù���, �� ID
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

    [Header("���� ���� ���")]
    public int Pink_point;
    public int Blue_point;

    void Start()
    {

    }

    void Update()
    {

    }
}
