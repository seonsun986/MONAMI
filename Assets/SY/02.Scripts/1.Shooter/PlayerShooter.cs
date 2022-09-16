using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ʿ�Ӽ� : ��ũ����, �߻���ġ

public class PlayerShooter : MonoBehaviour
{
    //������ġ
    public Transform firePos;
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

    void Start()
    {
    }
    void Update()
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
    private void InkShot()
    {
        Vector3 pos = Camera.main.transform.position;
        pos  =  pos+Camera.main.transform.forward * distance;

        Vector3 dir = (pos - firePos.position).normalized; 

        GameObject ink = Instantiate(InkFactory);
        ink.transform.position = firePos.position;
        ink.transform.forward = dir;
    }
}
