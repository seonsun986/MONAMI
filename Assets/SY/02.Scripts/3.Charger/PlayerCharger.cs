using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//사용자가 마우스 왼쪽 버튼을 누르면
//기를 모으고(빛나는 파티클 재생)
//마우스 버튼을 떼면 HitInfo의 방향으로 발사체를 발사시켜준다.
//히트인포와 나와의 거리를 구한 뒤
//바닥에 눈에 보이지 않는 콜라이더를 앞방향으로 쫘르륵 굴려주며 잉크를 묻힌다!
public class PlayerCharger : MonoBehaviour
{
    //쏘았는가
    bool isAttack = false;

    public GameObject VFX_Charging;
    public GameObject VFX_HitImpact;

    //TEST
    public GameObject test_Image;

    // crosshair
    public Image crosshair;

    void Start()
    {
        VFX_Charging.SetActive(false);
        crosshair.gameObject.SetActive(false);
        crosshair.fillAmount = 0;
    }

    RaycastHit hitInfo;

    // 게이지를 위한 것
    float currentVelocity = 0;

    // 마우스를 누르면 currentFill에 현재 Fillamount를 넣는다
    // chargetime이 지나면 Lerp로 currentFill에서 wantFill로 넣는다
    // 현재시간을 초기화 한다
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            crosshair.gameObject.SetActive(true);
            //StartCoroutine(IeCharge());
            float currentAmount = Mathf.SmoothDamp(crosshair.fillAmount, crosshair.fillAmount + 0.01f, ref currentVelocity, 2 * Time.deltaTime);
            crosshair.fillAmount = currentAmount;
            if (crosshair.fillAmount > 1)
            {
                crosshair.fillAmount = 1;
            }
            isAttack = true;
            hitInfo = Charging();
            //데미지를 중첩시켜줌(minDamage=>maxDamage)
        }
        //쏘았을 때만 플레이
        if (Input.GetMouseButtonUp(0))
        {
            crosshair.gameObject.SetActive(false);
            crosshair.fillAmount = 0;
            ChargerShot(hitInfo);
            //보이지않는 콜라이더 transform.pos => hitInfoPos까지 바닥에 깔아준다.
            isAttack = false;
            VFX_Charging.SetActive(false);
            

        }
    }

    RaycastHit Charging()
    {
        //기모으는 파티클 재생
        VFX_Charging.SetActive(true);
        //메인카메라의 위치에서 메인카메라의 앞방향으로 시선을 만들고 싶다.
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //print(hitInfo.transform.name);
            //잉크자국을 부딪힌 곳에 남기고싶다.
        }
        return hitInfo;
    }
    void ChargerShot(RaycastHit hitInfo)
    {
        GameObject inkImpact = Instantiate(VFX_HitImpact);
        print(hitInfo.transform.name);
        inkImpact.transform.position = hitInfo.point;
        inkImpact.transform.forward = hitInfo.normal;
    }
}

