using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Orb의 부모의 들어간 스크립트로 Y축을 기점으로 회전시켜준다.
public class Orb_Ink_Father_Rot : MonoBehaviour
{
    [Header("Rotate")]
    [SerializeField] float orbSpeed; //회전 속도

    [SerializeField] GameObject[] child_Ink;

    void Awake()
    {
        for (int i = 0; i < child_Ink.Length; i++)
        {
            child_Ink[i].SetActive(false);
        }
    }

    void Update()
    {
        //자식의 오브젝트가 켜져있다면 돌아가
        for (int i = 0; i < child_Ink.Length; i++)
        {
            child_Ink[i].SetActive(true);
        }
       
        //오브젝트를 Y축을 기점으로 스피드만큼회전 시킨다.
        transform.Rotate(new Vector3(0, 1, 0) * orbSpeed * Time.deltaTime);
    }
}
