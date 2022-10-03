using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��� ī�޶� ����ũ Ŭ������ �θ� Ŭ����
public class CS_Sine : CameraShakeBase

{
    float theta = 0;

    //ī�޶����ũ ���
    // transform : ī�޶����ũ�� ī�޶� Ʈ������
    // info : ����ڰ� ������ ī�޶����ũ ����
    public override void Play(Transform transform, CameraShakeInfo info)
    {
        // P = P0 +vt
        theta += info.sinSpeed * Time.deltaTime;
        transform.position = originPos + Vector3.up * Mathf.Sin(theta) * info.amplitude;
    }
    //ī�޶����ũ ����
    // transform : ī�޶����ũ�� ī�޶� Ʈ������
    public override void Stop(Transform transform)
    {
        transform.position = originPos;
        theta = 0;
    }
}
