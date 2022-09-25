using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraMovement : MonoBehaviourPun
{
    //따라가야 할 오브젝트의 정보
    public Transform objectTofollow;
    //따라갈 스피드
    public float followSpeed = 10f;
    //마우스 감도
    public float sensitivity = 100f;
    //카메라 상하로 움직일 때 제한 각도
    public float clampAngle = 70f;

    //마우스 인풋을 받을 변수
    private float rotX;
    private float rotY;

    //카메라 정보
    public Transform realCamera;
    //방향 : 크기와 방향을 둘 다 가지고 있음.
    public Vector3 dirNomalized;
    //최종적으로 정해진 방향을 저장해줄 벡터 값
    public Vector3 finalDir;
    //최소거리
    public float minDistance;
    //최대거리
    public float maxDistance;
    //최종적으로 결정된 거리
    public float finalDistace;
    //부드러움의 정도
    public float smoothness = 10f;

    //카메라 줌
    public float zoomDistance;

    public GameObject cam;

    void Start()
    {
        if (photonView.IsMine == true)
        {
            cam.SetActive(true);
        }
        //처음 시작할 때 각도를 초기화
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        //백터값도 초기화, 노멀라이즈를 하면 크기가 0으로 되서 방향만 저장
        dirNomalized = realCamera.localPosition.normalized;
        //magnitude : 크기
        finalDistace = realCamera.localPosition.magnitude;

        //게임씬에서 마우스 안 거슬리게
        /* Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;*/
    }

    void Update()
    {
        //매프레임마다 인풋을 받기 위함
        //X축을 기준으로 카메라가 움직일 때는 마우스를 상하로 움직이니 
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        //rotX : 카메라의 상하 회전의 값을 -70~70으로 한정지어준다
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        //카메라의 회전을 rot만큼 움직여준다
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    void LateUpdate()
    {

        //따라가게
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed * Time.deltaTime);
        //로컬스페이스에서 월드스페이스로 바꿔줌 (방향 x 최대거리);
        finalDir = transform.TransformPoint(dirNomalized * (maxDistance - zoomDistance));

        RaycastHit hit;
        Debug.DrawRay(transform.position, finalDir, Color.red);
        int layer = 1 << LayerMask.NameToLayer("Wall");
        if (Physics.Linecast(transform.position, finalDir, out hit, layer))
        {
            //만약에 라인을 그렸을 때 뭐가 있으면 (맞은 곳의 거리->최소거리)
            finalDistace = Mathf.Clamp(hit.distance, (minDistance - zoomDistance), (maxDistance - zoomDistance));
        }
        else
        {
            //만약에 뭐가 없다면 그냥 최대거리를 반영해줌.
            finalDistace = maxDistance - zoomDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNomalized * finalDistace, Time.deltaTime * smoothness);
    }
}
