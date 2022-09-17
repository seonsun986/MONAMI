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
    public int count;
    public int maxCount;
    void Start()
    {
        lowInkUI.SetActive(false);
        canShoot = true;
    }

    // � �Ŵ� �����ϴ� �Ŷ� ����UI�� count�� ����ȭ��Ų�� // 100�� �ִ� 
    public RectTransform uiInk; // �ִ� ������ : 2.37, ������ �ʾ����� ���� ������ �����Ѵ�
    public Transform inkTank;   // �ִ� ������ : 1

    void Update()
    {
        // ��ũ���� UI�� �����ִٸ�
        if(uiInk.gameObject.activeSelf == true)
        {
            if(uiInk.localScale.y > 0)
            {
                float uiYscale = (maxCount - count) * 0.0237f;
                uiInk.localScale = new Vector3(uiInk.localScale.x, uiYscale, uiInk.localScale.z);
            }
            
        }

        float inkTankYScale = 0.01f * (maxCount - count);
        inkTank.localScale = new Vector3(inkTank.localScale.x, inkTankYScale, inkTank.localScale.z);

        // ��ũ ��ũ 
        // �� �� ���� �ϱ�
        if (count > maxCount)
        {
            if (lowInkUI.activeSelf == false)
            {
                lowInkUI.SetActive(true);
            }
            // ���� �ʰ��ϱ�
            count = maxCount;
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

    // ���� �� �Ѿ� count 0���� �ǻ츮�� ���� ��
    // �� �� ���� ��(canShoot == false)
    // 0.1�ʿ� 2���� �Ѿ� �����ǵ��� �Ѵ�
    // maxcount���� count�� �������� maxCount�� �ٽ� �ϰ� �Ѵ�
    // �ʿ�Ӽ� : canShoot, 0.1�ʸ���  ��������, maxCount, �����ð�, ����ð�

    

    [Header("�Ѿ� ������ ���� ����")]
    float currentTime;              // ���� �ð�
    public float chargerTime = 0.1f;   // ���� �ð�
    public int chargeBullet = 2; // 0.1�� ���� ���� ����
    public void ChargeInk()
    {
        currentTime += Time.deltaTime;
        if (currentTime > chargerTime)
        {
            if (count <= 0)
            {
                return;
            }
            // ī��Ʈ�� �߰� ��Ų��
            count -= chargeBullet;            
            currentTime = 0;
        }
    }
}
