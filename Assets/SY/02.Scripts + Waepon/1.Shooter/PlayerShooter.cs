using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


//�ʿ�Ӽ� : ��ũ����, �߻���ġ

public class PlayerShooter : MonoBehaviourPun
{
    //������ġ
    public GameObject firePos;
    //��ũ����
    public GameObject InkFactory;
    //��Ÿ�
    public float distance;
    //���� ��ũ �߻�ð�
    private float nextFire = 0.0f;
    //��ƼŬ
    [SerializeField] ParticleSystem inkParticle;

    public Camera cam;

    // ��ũ���� ���� ���� �͵�

    public bool canShoot;
    public bool hideCanShoot;       // ������ �� ����� �Ѵ�
    public GameObject crossHair;
    public GameObject lowInkUI;
    public int count;
    public int maxCount;
    CanHide canHide;
    Player_HP hp;
    OrbGauge orb;

    [Header("����Ʈ ������ ����")]
    public GameObject gauge;
    public Text pointTxt;              // ���ʹ� ����Ʈ 1���� �ø���
    public int point;
    public int maxPoint = 300;
    public Image FillGauge;         // �������� FillAmount�� 0.8�� �ִ�� �Ѵ�.
    // �Ҹ�
    public AudioSource plzRefillInk;
    void Start()
    {
        // GameManager���� ���� photonView�� ����
        GameManager.Instance.CountPlayer(photonView);
        canHide = GetComponent<CanHide>();
        lowInkUI.SetActive(false);
        canShoot = true;
        crossHair.SetActive(false);
        pointTxt.gameObject.SetActive(false);
        gauge.SetActive(false);
        hp = GetComponent<Player_HP>();
        orb = GetComponent<OrbGauge>();
        
    }

    // � �Ŵ� �����ϴ� �Ŷ� ����UI�� count�� ����ȭ��Ų�� // 50�� �ִ� 
    public RectTransform uiInk; // �ִ� ������ : 2.37, ������ �ʾ����� ���� ������ �����Ѵ�
    public Transform inkTank;   // �ִ� ������ : 1
    float currentTime2;
    public float delayTime = 0.2f;

    void Update()
    {
        if (orb.isOrb == true) return;
        // �����̶��
        if (photonView.IsMine)
        {
            if (GameStateManager.gameState.gstate != GameStateManager.GameState.Go) return;
            if (hp.hp <= 0) return;
            if (crossHair.activeSelf == false) crossHair.SetActive(true);
            if (gauge.activeSelf == false) gauge.SetActive(true);
            if (pointTxt.gameObject.activeSelf == false) pointTxt.gameObject.SetActive(true);
            // ��ũ���� UI�� �����ִٸ�
            if (uiInk.gameObject.activeSelf == true)
            {
                if (uiInk.localScale.y >= 0)
                {
                    float uiYscale = (maxCount - count) * 0.0474f;
                    uiInk.localScale = new Vector3(uiInk.localScale.x, uiYscale, uiInk.localScale.z);
                }

                if (uiInk.localScale.y > 2.37f)
                {
                    uiInk.localScale = new Vector3(uiInk.localScale.x, 2.37f, uiInk.localScale.z);

                }
            }
            // �� �� ������ ���°� �Ǹ�
            // UI�� ������ �ص� ������ ���� �ʴ´�
            float inkTankYScale = 0.02f * (maxCount - count);
            inkTank.localScale = new Vector3(inkTank.localScale.x, inkTankYScale, inkTank.localScale.z);

            // ��ũ ��ũ 
            // �� �� ���� �ϱ�
            if (count >= maxCount || hideCanShoot == false)
            {
                // ��ũ�� ��ġ�� ������ �����
                // �������� �� ������ �����
                // ��ũ����! UI ����
                // ��ũ ������ �������� ��Ÿ���� ���� �ʱ�!
                if (count >= maxCount && lowInkUI.activeSelf == false)
                {
                    lowInkUI.SetActive(true);
                    // ���� �ʰ��ϱ�
                    count = maxCount;
                    if(!plzRefillInk.isPlaying)
                    {
                        plzRefillInk.Play();
                    }
                }
                
                canShoot = false;
            }

            else
            {
                // ��ũ����! UI ���ֱ�
                if (lowInkUI.activeSelf == true)
                {
                    lowInkUI.SetActive(false);
                }
                // ���� �ʾ������� �� �� �ִ�
                canShoot = true;
            }

            if (canShoot == true)
            {
                //���콺 ���ʹ�ư�� ������
                //Time.time �Լ��� nexFire ������ Ŭ ���� ����
                if (Input.GetMouseButton(0))
                {
                    currentTime2 += Time.deltaTime;
                    if(currentTime2 > delayTime)
                    {
                        //��ũ��ƼŬ ���
                        inkParticle.Play();
                        photonView.RPC("RPCShowBullet", RpcTarget.All,cam.transform.position, cam.transform.forward);
                        // ����Ʈ �ݿ�
                        point++;
                        pointTxt.text = point.ToString() + " P";
                        FillGauge.fillAmount = (float)point / (float)maxPoint;
                        currentTime2 = 0;
                    }

                    
                    //InkShot();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    inkParticle.Stop();
                }
            }
            // �� �� ���� ��
            else
            {
                if (inkParticle.isPlaying)
                {
                    inkParticle.Stop();
                }

            }
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
            Vector3 vo = CalculateVelocity(hitInfo.point, firePos.transform.position, 0.2f);
             GameObject ink =  PhotonNetwork.Instantiate("Shooter_Ink", firePos.transform.position, firePos.transform.rotation);
            //GameObject ink = Instantiate(InkFactory); 
            //ink.transform.position = firePos.transform.position;
            //ink.transform.forward = firePos.transform.forward;
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

    public GameObject pinkBullet;
    public GameObject blueBullet;
    [PunRPC]
    public void RPCShowBullet(Vector3 position, Vector3 forward)
    {
        count++;
        // ī�޶� ���߾����� ���̸� ���.
        Ray ray = new Ray(position, forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 vo = CalculateVelocity(hitInfo.point, firePos.transform.position, 0.2f);
            // �÷��̾ ��ٶ��
            if(gameObject.name.Contains("Pink"))
            {
                GameObject ink = Instantiate(pinkBullet);
                ink.transform.position = firePos.transform.position;
                ink.transform.forward = firePos.transform.forward;
                ink.GetComponent<Rigidbody>().velocity = vo;
            }
            else
            {
                GameObject ink = Instantiate(blueBullet);
                ink.transform.position = firePos.transform.position;
                ink.transform.forward = firePos.transform.forward;
                ink.GetComponent<Rigidbody>().velocity = vo;
            }            
        }
    }
}
