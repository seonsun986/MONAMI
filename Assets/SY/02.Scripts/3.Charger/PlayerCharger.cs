using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자가 마우스 왼쪽 버튼을 누르면
//기를 모으고(빛나는 파티클 재생)
//마우스 버튼을 떼면 HitInfo의 방향으로 발사체를 발사시켜준다.
//히트인포와 나와의 거리를 구한 뒤
//바닥에 눈에 보이지 않는 콜라이더를 앞방향으로 쫘르륵 굴려주며 잉크를 묻힌다!
public class PlayerCharger : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Charging();
            //기모으는 파티클 재생
            //데미지를 중첩시켜줌(minDamage=>maxDamage)
        }
        if(Input.GetMouseButtonUp(0))
        {
            ChargerShot();
            //보이지않는 콜라이더 transform.pos => hitInfoPos까지 바닥에 깔아준다.
        }
    }
    void Charging()
    {

    }
    void ChargerShot()
    {

    }
}
