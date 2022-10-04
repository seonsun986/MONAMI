using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultInfo
{
    public string nickName;
    public string weapon;
    public int id;
}

//내가 선택무기, 내 ID
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string weaponName;
    public int id;
    public string nickname;
    [Header("영역 판정 결과")]
    public int Pink_point;
    public int Blue_point;

    
    void Start()
    {

    }

    void Update()
    {

    }

    public List<ResultInfo> resultInfos = new List<ResultInfo>();
    public void SetResultInfo()
    {
        resultInfos.Clear();

        ResultInfo resultInfo;
        SSH_Player player;
        for(int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            player = GameManager.Instance.players[i].GetComponent<SSH_Player>();
            resultInfo = new ResultInfo();

            resultInfo.id = player.id;
            resultInfo.weapon = player.weaponName;
            resultInfo.nickName = GameManager.Instance.players[i].Owner.NickName;

            resultInfos.Add(resultInfo);
        }
    }
}
