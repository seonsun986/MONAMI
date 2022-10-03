using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Player�� ������ �ϸ� CurrentGauge��½����ְ�,
// CurrentGauge�� MaxGauge�� �Ǹ� => ���� á�ٴ� �ð����� �̱��̱�Ȱ��ȭ, 
public class OrbGauge : MonoBehaviourPun
{
    [Header("ORB")]
    [SerializeField] GameObject _OrbLine;
    [SerializeField] GameObject _OrbInk;
    [SerializeField] GameObject _OrbChargingImpact;
    [SerializeField] GameObject _HitPoint;

    //��ư�� �ѹ� ���� ���� �ѹ� �� ���� �� (Ȱ��ȭ/��Ȱ��ȭ)
    int rButtonCount = 0;

    [Header("Gauge")]
    public float currentGauge;
    [SerializeField] float maxGauge = 1;

    public Camera cam;
    public bool isOrb;

    void Start()
    {
        //�ӽ÷� 1�� �־��༭ �÷��� �����ϰ� �������
        currentGauge = 1;
        _OrbLine.SetActive(false);
        _OrbInk.SetActive(false);
        _OrbChargingImpact.SetActive(false);
        _HitPoint.SetActive(false);
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        //MaxGauege�� ��� ä���ٸ�
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
        //MaxGauge�� �ƴ϶��
        else
        {
            //����� �� ���ٴ� ƽ!ƽ! �Ҹ� ���ֱ�.
        }
    }

    //Ȱ��ȭ
    void OrbOn()
    {
        cam.GetComponentInParent<CameraMovement>().zoomDistance = 3f;
        //�÷��̾� �⺻���� ��Ȱ��ȭ �����ֱ�.
        _OrbLine.SetActive(true);
        _OrbInk.SetActive(true);
        _OrbChargingImpact.SetActive(true);
        _HitPoint.SetActive(true);
        rButtonCount++;
    }

    //��Ȱ��ȭ
    void OrbOff()
    {
        cam.GetComponentInParent<CameraMovement>().zoomDistance = 0f;
        //�÷��̾� �⺻���� Ȱ��ȭ �����ֱ�.
        _OrbLine.SetActive(false);
        _OrbInk.SetActive(false);
        _OrbChargingImpact.SetActive(false);
        _HitPoint.SetActive(false);
        rButtonCount++;
    }
}
