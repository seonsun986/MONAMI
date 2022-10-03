using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Orb_Line : MonoBehaviour
{
    [SerializeField] float _InitialVelocity;
    [SerializeField] float _Angle;
    [SerializeField] float _Step;
    [SerializeField] LineRenderer _Line;
    [SerializeField] Transform _FirePos;
    //������ ���� ���������� ��ƼŬ
    [SerializeField] GameObject _HitPoint;
    [SerializeField] Camera _cam;

    [SerializeField] GameObject orb_Line;
    [SerializeField] GameObject trailRenderer;
    [SerializeField] GameObject orb_charging;

    private void Start()
    {
        //�߻��������� �浹�����ʰ� �ݶ��̴� ���ֱ�.
        GetComponent<SphereCollider>().enabled = false;
        //Hit��ġ�� ��ǥ���� ������ ���������ֱ�
        _HitPoint.SetActive(true);
        //Ʈ���Ϸ������� �߻����� ���� ���ֱ�.
        trailRenderer.SetActive(false);
        orb_charging.SetActive(true);
    }
    int count;
    public void Update()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 direction = hitInfo.point - _FirePos.position;
            Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
            Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);
            float height = targetPos.y + targetPos.magnitude / 2f;
            height = Mathf.Max(0.01f, height);
            float angle;
            float v0;
            float time;

            CalculatePathWithHeight(targetPos, height, out v0, out angle, out time);

            DrawPath(groundDirection.normalized, v0, angle, time, _Step);
            if (Input.GetMouseButtonDown(0))
            {
                orb_charging.SetActive(false);
                trailRenderer.SetActive(true);
                //�浹�� �� �ְ� �ݶ��̴� ���ش�.
                GetComponent<SphereCollider>().enabled = true;

                StopAllCoroutines();
                StartCoroutine(IE_Movement(groundDirection.normalized, v0, angle, time));

                //�ʻ�� ������ 0���� ������ֱ� OrbGauge.cs���� ���� �޾ƿ���.
                _cam.GetComponentInParent<OrbGauge>().currentGauge = 0;
                _cam.GetComponentInParent<OrbGauge>().isOrb = false;

                //�ٽ� �� �÷��̾� ������ �� �ְ� ���ֱ� �� �÷��̾� �⺻������ Ȱ��ȭ ��������!

                orb_Line.SetActive(false);
                _HitPoint.SetActive(false);

            }
        }
        _HitPoint.transform.position = hitInfo.point;
        _HitPoint.transform.up = hitInfo.normal;

    }
    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step)
    {
        step = Mathf.Max(0.01f, step);
        _Line.positionCount = (int)(time / step) + 2;
        int count = 0;
        for (float i = 0; i < time; i += step)
        {
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _Line.SetPosition(count, _FirePos.position + direction * x + Vector3.up * y);
            count++;
        }
        float xFinal = v0 * time * Mathf.Cos(angle);
        float yFinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _Line.SetPosition(count, _FirePos.position + direction * xFinal + Vector3.up * yFinal);
    }

    private float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b * time / xt);

        v0 = b / Mathf.Sin(angle);
    }

    private void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }
    IEnumerator IE_Movement(Vector3 direction, float v0, float angle, float time)
    {
        float t = 0;
        while (t < time)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _FirePos.position + direction * x + Vector3.up * y;

            t += Time.deltaTime;
            yield return null;
        }
    }
}
