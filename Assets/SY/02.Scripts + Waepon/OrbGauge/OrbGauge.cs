using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Player가 공격을 하면 CurrentGauge상승시켜주고,
// CurrentGauge가 MaxGauge가 되면 => 궁이 찼다는 궁게이지 이글이글활성화, 
public class OrbGauge : MonoBehaviourPun
{
    [Header("ORB")]
    [SerializeField] GameObject _OrbLine;
    [SerializeField] GameObject _OrbInk;
    [SerializeField] GameObject _OrbChargingImpact;
    [SerializeField] GameObject _HitPoint;

    //버튼을 한번 누를 때와 한번 더 누를 때 (활성화/비활성화)
    int rButtonCount = 0;

    [Header("Gauge")]
    public float currentGauge;
    [SerializeField] float maxGauge = 1;

    public Camera cam;
    public bool isOrb;

    void Start()
    {
        //임시로 1로 넣어줘서 플레이 가능하게 만들어줌
        currentGauge = 1;
        _OrbLine.SetActive(false);
        _OrbInk.SetActive(false);
        _OrbChargingImpact.SetActive(false);
        _HitPoint.SetActive(false);
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        //MaxGauege를 모두 채웠다면
        if (currentGauge >= maxGauge)
        {
            if (Input.GetKeyDown(KeyCode.R) && rButtonCount % 2 == 0)
            {
                isOrb = true;
                OrbOn();
            }
            else if (Input.GetKeyDown(KeyCode.R) && rButtonCount % 2 == 1)
            {
                isOrb = false;
                OrbOff();
            }
        }
        //MaxGauge가 아니라면
        else
        {
            //사용할 수 없다는 틱!틱! 소리 내주기.
        }
    }

    //활성화
    void OrbOn()
    {
        cam.GetComponentInParent<CameraMovement>().zoomDistance = 3f;
        //플레이어 기본공격 비활성화 시켜주기.
        _OrbLine.SetActive(true);
        _OrbInk.SetActive(true);
        _OrbChargingImpact.SetActive(true);
        _HitPoint.SetActive(true);
        rButtonCount++;
    }

    //비활성화
    void OrbOff()
    {
        cam.GetComponentInParent<CameraMovement>().zoomDistance = 0f;
        //플레이어 기본공격 활성화 시켜주기.
        _OrbLine.SetActive(false);
        _OrbInk.SetActive(false);
        _OrbChargingImpact.SetActive(false);
        _HitPoint.SetActive(false);
        rButtonCount++;
    }
}
