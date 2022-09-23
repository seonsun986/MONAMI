using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� ī�޶� ����ũ Ŭ������ �θ� Ŭ����
public class CS_Random : CameraShakeBase

{
    
    //ī�޶����ũ ���
    // transform : ī�޶����ũ�� ī�޶� Ʈ������
    // info : ����ڰ� ������ ī�޶����ũ ����
    public override void Play(Transform transform, CameraShakeInfo info)
    {
        transform.position = originPos + Random.insideUnitSphere * info.amplitude * Time.deltaTime;
    }
    //ī�޶����ũ ����
    // transform : ī�޶����ũ�� ī�޶� Ʈ������
    public override void Stop(Transform transform)
    {
        transform.position = originPos;
    }
}
