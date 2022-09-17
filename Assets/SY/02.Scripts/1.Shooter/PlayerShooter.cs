using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ʿ�Ӽ� : ��ũ����, �߻���ġ

public class PlayerShooter : MonoBehaviour
{
    //������ġ
    public GameObject firePos;
    //��ũ����
    public GameObject InkFactory;
    //��Ÿ�
    public float distance;
    //��ũ ���� �ð� ����
    private float fireRate = 0.1f;
    //���� ��ũ �߻�ð�
    private float nextFire = 0.0f;
    //��ƼŬ
    [SerializeField] ParticleSystem inkParticle;

    public Camera cam;

    // ��ũ���� ���� ���� �͵�

    public bool canShoot;
    public GameObject lowInkUI;
    int count;
    public int maxCount;
    void Start()
    {
        
        canShoot = true;
    }
    void Update()
    {
        if (count > maxCount)
        {
            canShoot = false;
        }

        if(canShoot == true)
        {
            //���콺 ���ʹ�ư�� ������
            //Time.time �Լ��� nexFire ������ Ŭ ���� ����
            if (Input.GetMouseButton(0) && Time.time > nextFire)
            {

                inkParticle.Play();
                //��ũ��ƼŬ ���
                nextFire = Time.time + fireRate;
                InkShot();
            }
            else if (Input.GetMouseButtonUp(0))
                inkParticle.Stop();
        }
        else
        {
            return;
        }
        
    }
    private void InkShot()
    {
        count++;
        // ī�޶� ���߾����� ���̸� ���.
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo))
        {
            Vector3 vo = CalculateVelocity(hitInfo.point, firePos.transform.position, 0.5f);
            GameObject ink = Instantiate(InkFactory);
            ink.transform.position = firePos.transform.position;
            ink.transform.forward = firePos.transform.forward;
            ink.GetComponent<Rigidbody>().velocity = vo;
        }
        //Vector3 pos = Camera.main.transform.position;
        //pos  =  pos+Camera.main.transform.forward * distance;

        //Vector3 dir = (pos - firePos.position).normalized; 

        //ink.transform.position = firePos.position;
        //ink.transform.forward = firePos.forward;
    }

    public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        // x, y ���̸� ���� ���Ѵ�
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;
        // ���̸� ����ϴ� float����
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
