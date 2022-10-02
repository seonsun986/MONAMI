using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Orb�� �θ��� �� ��ũ��Ʈ�� Y���� �������� ȸ�������ش�.
public class Orb_Ink_Father_Rot : MonoBehaviour
{
    [Header("Rotate")]
    [SerializeField] float orbSpeed; //ȸ�� �ӵ�

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
        //�ڽ��� ������Ʈ�� �����ִٸ� ���ư�
        for (int i = 0; i < child_Ink.Length; i++)
        {
            child_Ink[i].SetActive(true);
        }
       
        //������Ʈ�� Y���� �������� ���ǵ常ŭȸ�� ��Ų��.
        transform.Rotate(new Vector3(0, 1, 0) * orbSpeed * Time.deltaTime);
    }
}
