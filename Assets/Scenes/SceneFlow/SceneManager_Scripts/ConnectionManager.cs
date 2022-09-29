using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//��Ʈ��ũ�� ����ϱ� ���ؼ� / ����Ÿ���� ����ؾ��� ���� ������ ��κ� Pun�� ����ϸ� �ȴ�.
using Photon.Pun;
using UnityEngine.UI;
using OpenCvSharp;

public class ConnectionManager : MonoBehaviourPunCallbacks
//MonoBehaviourPunCallbacks : 
{
    public Material screenMaterial;
    // �г��� InputField
    public InputField inputNickname;
    // ���� Button
    public Button btnConnect;

    void Start()
    {
        screenMaterial.SetFloat("_FullscreenIntensity", 0f);
        btnConnect.interactable = false;
        // �г�����(InputField) ����� �� ȣ��Ǵ� �Լ� ���
        inputNickname.onValueChanged.AddListener(OnValueChanged);
        //�г�����(InputField) Focusing�� �Ҿ��� �� ȣ��Ǵ� �Լ� ���
        inputNickname.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnValueChanged(string s)        // ����� ������ ȣ���
    {
        // ���࿡ s�� ���̰� 0���� ũ�ٸ� 
        // ���� ��ư�� Ȱ��ȭ ����
        // �׷��� �ʴٸ� 
        // �ٽ� ���ӹ�ư�� ��Ȱ��ȭ����
        btnConnect.interactable = s.Length > 0;
        print("OnValueChanged : " + s);
    }

    public void OnEndEdit(string s)
    {
        print("OnEditEnd : " + s);
    }
    public void OnClickConnect()
    {
        // NameServer�� ����(APPid,GameVersion, ����)
        PhotonNetwork.ConnectUsingSettings(); // ���� ���� ������ ������ �����ϰڴ�(�ش� �Լ� static)

    }


    //������ ������ ���� ����, �κ񸸵�ų� ������ �� ���� ����
    public override void OnConnected()
    {
        base.OnConnected();
        print("������ ������ ���� ����!" + "/ OnConnected");

    }
    //������ ������ ����, �κ� ���� �� ������ ������ ���°� �� ��.
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������ ������ ���� ����!" + "/ OnConnectedToMaster");

        //�г��� ����
        PhotonNetwork.NickName = inputNickname.text;
        //�⺻ �κ� ���� / ä�ΰ���
        PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("�κ�1", Photon.Realtime.LobbyType.Default));
        //PhotonNetwork.JoinLobby();
        //Ư�� �κ� ���� -- ä�� ������ ���� ��
        //PhotonNetwork.JoinLobby(new Photon.Realtime.TypedLobby("�κ�",Photon.Realtime.LobbyType.Default))
    }

    //�κ� ���� ������ ȣ��
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� ����!" + "/ OnJoinedLobby");

        //LobbyScene���� �̵�
        PhotonNetwork.LoadLevel("02.Lobby");
    }

    public Texture2D texture;

    void Update()
    {
        //Load texture
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Mat image = OpenCvSharp.Unity.TextureToMat(texture);

            //RGB�� ����
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
