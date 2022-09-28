using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    //싱글톤 변수
    public static GameStateManager gameState;

    private void Awake()
    {
        if (gameState == null)
        {
            gameState = this;
        }
    }

    public enum GameState
    {
        Ready,
        Go,
        GameOver
    }

    public GameObject ready;
    public GameObject go;
    public GameState gstate;

    public GameObject start_UI;
    void Start()
    {
        gstate = GameState.Ready;
    }

    int count;
    void Update()
    {
        if (start_UI.activeSelf == false)
        {
            // GameState가 go가 아닐때 해당 state실행시키고 싶다
            if (gstate == GameState.Go) return;

            else
            {
                if (count < 1)
                {
                    StartCoroutine(ReadyToStart());
                    count++;
                }
            }
        }

    }

    IEnumerator ReadyToStart()
    {
        ready.SetActive(true);
        //2초간 대기한다.
        yield return new WaitForSeconds(2f);

        ready.SetActive(false);
        //'Go!'로 한다.
        go.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        go.SetActive(false);
        gstate = GameState.Go;

        GameManager.Instance.StartCountDown();

    }

}
