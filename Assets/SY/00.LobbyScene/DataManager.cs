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
    public string nickname;
    [Header("영역 판정 결과")]
    public int Pink_point;
    public int Blue_point;

    void Start()
    {

    }

    void Update()
    {

    }
}
