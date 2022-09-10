using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Weapon : MonoBehaviour
{
    float weaponDamage = 1f;
    void Start()
    {

    }

    void Update()
    {

    }
  
    void OnTriggerEnter(Collider other)
    {
        //가정) 맞은 대상 게임오브젝트가 Player라는 태그를 하고 있다면
        if (other.gameObject.CompareTag("Player"))
        {
            print("부딪혔다!!");
            //부딪힌 대상의 PlayerHP를 가져와
            PlayerHP php = other.transform.GetComponent<PlayerHP>();
            if (php != null)
            {
                //PlayerHP에 있는 OnDamage 함수 실행
                php.OnDamaged(weaponDamage);
            }
        }
    }
}
