using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance;

    [SerializeField] GameObject player, enemy;
    [SerializeField] GameObject pathGenerator;
    [SerializeField] GameObject startSpace;

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        pathGenerator.SetActive(true);
    }


}
