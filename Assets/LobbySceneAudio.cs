using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneAudio : MonoBehaviour
{

    public static LobbySceneAudio instance;
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
    void Start()
    {

    }

    void Update()
    {
        
    }
}
