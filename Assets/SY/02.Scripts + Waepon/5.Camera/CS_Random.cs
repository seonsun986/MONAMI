using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 카메라 셰이크 클래스의 부모 클래스
public class CS_Random : CameraShakeBase

{
    
    //카메라셰이크 재생
    // transform : 카메라셰이크할 카메라 트랜스폼
    // info : 사용자가 설정한 카메라셰이크 정보
    public override void Play(Transform transform, CameraShakeInfo info)
    {
        transform.position = originPos + Random.insideUnitSphere * info.amplitude * Time.deltaTime;
    }
    //카메라셰이크 정지
    // transform : 카메라셰이크할 카메라 트랜스폼
    public override void Stop(Transform transform)
    {
        transform.position = originPos;
    }
}
