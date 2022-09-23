using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자가 셰이크를 편집할 수 있는 설정 정보 구조체
[System.Serializable]
public struct CameraShakeInfo
{
    //진폭
    public float amplitude;
}
//모든 카메라 셰이크 클래스의 부모 클래스
public class CameraShakeBase
{
    //카메라의 초기위치 기억
    protected Vector3 originPos;

    //카메라셰이크 초기화 함수
    public virtual void Init(Vector3 originPos)
    {
        this.originPos = originPos;
    }
    //카메라셰이크 재생
    // transform : 카메라셰이크할 카메라 트랜스폼
    // info : 사용자가 설정한 카메라셰이크 정보
    public virtual void Play(Transform transform, CameraShakeInfo info)
    {

    }
    //카메라셰이크 정지
    // transform : 카메라셰이크할 카메라 트랜스폼
    public virtual void Stop(Transform transform)
    {

    }
}
