using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController: MonoBehaviour{
    
    //정해진 색깔을
    public Color paintColor;
    //범위
    public float radius = 1;
    //강도
    public float strength = 1;
    //경도...??
    public float hardness = 1;

    

    private void OnCollisionStay(Collision other)
    {
        Paintable p = other.collider.GetComponent<Paintable>();
        if (p != null)
        {
            //contacts = 물리엔진에 의해 생성되는 물체 간의 충돌지점.
            //접점은 여러개가 될 수 있음-> 배열 타입 반환
            //contacts[0].point = 첫벗째 충돌 지점의 위치
            //즉 PaintManager에 있는 페인트를 부딪힌 위치에 생성시켜준다는 뜻!!
            Vector3 pos = other.contacts[0].point;
            PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
        }
    }
}