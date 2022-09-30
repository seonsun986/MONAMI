using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] Transform blackHole;

    [Header("Scale")]
    [SerializeField] float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float timeToMaxScale;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        StartCoroutine("OrbBomb");
    }
    IEnumerator OrbBomb()
    {
        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            blackHole.localScale = Vector3.one * Mathf.Lerp(blackHole.localScale.x, maxScale, f / timeToMaxScale);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            blackHole.localScale = Vector3.one * Mathf.Lerp(blackHole.localScale.x, 0, f / timeToMaxScale);
            yield return null;
        }

        gameObject.SetActive(false);
        Destroy(gameObject, 2f);
    }
}
