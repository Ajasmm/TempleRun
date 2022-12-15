using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningPathManager : MonoBehaviour
{
    public static RunningPathManager Instance;

    [SerializeField] GameObject[] straightPaths;
    [SerializeField] GameObject[] turnPaths;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

    }

    private void OnEnable()
    {
        
    }
}
