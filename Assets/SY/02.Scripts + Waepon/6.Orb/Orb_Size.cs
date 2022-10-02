using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb_Size : MonoBehaviour
{
    [Header("Child")]
    [SerializeField] GameObject child_Pos;
    [SerializeField] Transform child_Orb;

    [Header("Scale")]
    [SerializeField] float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float timeToMaxScale;

    [Header("궁극기 유지 시간")]
    [SerializeField] float holdingTime;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        child_Pos.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            //벽과 바닥에 닿으면 더 이상 영향을 받지않게 만들어주겠다.
            rb.useGravity = false;
            rb.isKinematic = true;

        }
        //스케일을 만들어줌.
        StartCoroutine("OrbBomb");
    }
    IEnumerator OrbBomb()
    {
        GetComponent<SphereCollider>().enabled = false;
        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            child_Orb.localScale = Vector3.one * Mathf.Lerp(child_Orb.localScale.x, maxScale, f / timeToMaxScale);
            yield return null;
        }

        //얼마동안 사이즈 크기를 유지할 것인가.
        child_Pos.SetActive(true);
        yield return new WaitForSeconds(holdingTime);
        child_Pos.SetActive(false);
        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            child_Orb.localScale = Vector3.one * Mathf.Lerp(child_Orb.localScale.x, 0, f / timeToMaxScale);
            yield return null;
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
