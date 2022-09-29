using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//네트워크를 사용하기 위해서 / 리얼타임을 사용해야할 일이 있지만 대부분 Pun을 사용하면 된다.
using Photon.Pun;
using UnityEngine.UI;
using OpenCvSharp;

public class ConnectionManager : MonoBehaviourPunCallbacks
//MonoBehaviourPunCallbacks : 
{
    public Material screenMaterial;
    // 닉네임 InputField
    public InputField inputNickname;
    // 접속 Button
    public Button btnConnect;

    void Start()
    {
        screenMaterial.SetFloat("_FullscreenIntensity", 0f);
        btnConnect.interactable = false;
        // 닉네임이(InputField) 변경될 때 호출되는 함수 등록
        inputNickname.onValueChanged.AddListener(OnValueChanged);
        //닉네임이(InputField) Focusing을 잃었을 때 호출되는 함수 등록
        inputNickname.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnValueChanged(string s)        // 변경될 때마다 호출됨
    {
        // 만약에 s의 길이가 0보다 크다면 
        // 접속 버튼을 활성화 하자
        // 그렇지 않다면 
        // 다시 접속버튼을 비활성화하자
        btnConnect.interactable = s.Length > 0;
        print("OnValueChanged : " + s);
    }

    public void OnEndEdit(string s)
    {
        print("OnEditEnd : " + s);
    }
    public void OnClickConnect()
    {
        // NameServer에 접속(APPid,GameVersion, 지역)
        PhotonNetwork.ConnectUsingSettings(); // 내가 가진 세팅을 가지고 접속하겠다(해당 함수 static)

    }


    //마스터 서버에 접속 성공, 로비만들거나 진입할 수 없는 상태
    public override void OnConnected()
    {
        base.OnConnected();
        print("마스터 서버에 접속 성공!" + "/ OnConnected");

    }
    //마스터 서버에 접속, 로비 생성 및 진입이 가능한 상태가 된 것.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 접속 성공!" + "/ OnConnectedToMaster");

        //닉네임 설정
        PhotonNetwork.NickName = inputNickname.text;
        //기본 로비 진입 / 채널개념
        PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("로비1", Photon.Realtime.LobbyType.Default));
        //PhotonNetwork.JoinLobby();
        //특정 로비 진입 -- 채널 나누고 싶을 때
        //PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("로비",Photon.Realtime.LobbyType.Default))
    }

    //로비 접속 성공시 호출
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 접속 성공!" + "/ OnJoinedLobby");

        //LobbyScene으로 이동
        PhotonNetwork.LoadLevel("02.Lobby");
    }

    public Texture2D texture;

    void Update()
    {
        //Load texture
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Mat image = OpenCvSharp.Unity.TextureToMat(texture);

            //RGB로 변경
            Cv2.CvtColor(image, image, ColorConversionCodes.BGR2RGB);

            Scalar red1 = new Scalar(255, 0, 0.5f * 255);
            Scalar red2 = new Scalar(255 *0.7f, 0, 0.5f * 255 * 0.7f);

            Mat dst = new Mat();
            Cv2.InRange(image, red2, red1, dst);
            int a = Cv2.CountNonZero(dst);

            red1 = new Scalar(0, 0.5f * 255, 255);
            red2 = new Scalar(0, 0.5f * 255 * 0.7f, 255 * 0.7f);

            dst = new Mat();
            Cv2.InRange(image, red2, red1, dst);
            a = Cv2.CountNonZero(dst);
        }
    }

}
