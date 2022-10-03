using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ī�޶����ũ�� ���۽�Ű�� Ŭ����
//�ʿ�Ӽ� : Ÿ��ī�޶�, ����ð�, ī�޶����ũ ����, ī�޶����ũŸ��, �����ų ī�޶����ũ Ŭ����

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private void Awake()
    {
        instance = this;
    }

    //Ÿ��ī�޶�
    public Transform targetCamera;
    //����ð�
    public float playTime = 0.1f;
    [SerializeField]
    //ī�޶����ũ ����
    CameraShakeInfo info;

    //ī�޶����ũŸ��
    public enum CameraShakeType
    {
        Random,
        Sine,
        Animation
    }
    public CameraShakeType cameraShakeType = CameraShakeType.Random;
    //�����ų ī�޶����ũ Ŭ����
    CameraShakeBase cameraShake;
    void Start()
    {
        cameraShake = CreateCameraShake(cameraShakeType);
        cameraShake.Init(targetCamera.position);
    }

    public static CameraShakeBase CreateCameraShake(CameraShakeType type)
    {
        switch (type)
        {
            case CameraShakeType.Random:
                return new CS_Random();
            case CameraShakeType.Sine:
                return new CS_Sine();
            case CameraShakeType.Animation:
                break;
        }
        return null;
    }

    void Update()
    {
      
    }
    public void PlayCameraShake()
    {
        //ī�޶����ũ Ÿ���� �ִϸ��̼��� �ƴ� ���
        if (cameraShakeType != CameraShakeType.Animation)
        {
            StopAllCoroutines();
            StartCoroutine(Play());
        }
    }

    //����ð����� ī�޶����ũ ����
    IEnumerator Play()
    {
        cameraShake.Init(targetCamera.position);

        float currentTime = 0;
        //����ð����� ī�޶����ũ ����
        while (currentTime < playTime)
        {
            currentTime += Time.deltaTime;
            //ī�޶� ����ũ ����
            cameraShake.Play(targetCamera, info);
            yield return null;
        }
        //������ Stop
        cameraShake.Stop(targetCamera);
    }
}
