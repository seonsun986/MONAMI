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

    [Header("�ñر� ���� �ð�")]
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
            //���� �ٴڿ� ������ �� �̻� ������ �����ʰ� ������ְڴ�.
            rb.useGravity = false;
            rb.isKinematic = true;

        }
        //�������� �������.
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

        //�󸶵��� ������ ũ�⸦ ������ ���ΰ�.
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
