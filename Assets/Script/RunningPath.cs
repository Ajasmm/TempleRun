using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RunningPath : MonoBehaviour
{
    [SerializeField] public PathType type = PathType.Straight;
    [SerializeField] Transform[] exitPaths;

    bool isPlayerInside = false;

    GameObject[] exitPathGameObjects;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnEnable()
    {
        isPlayerInside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InstanciateExitPaths();
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) Invoke("DisableThisPath", 2F);
    }

    private void InstanciateExitPaths()
    {
        exitPathGameObjects = new GameObject[exitPaths.Length];
        for(int i=0; i<exitPaths.Length; i++)
        {
            GameObject obj = PathGenerator.instance.GetPath();
            obj.transform.SetPositionAndRotation(exitPaths[i].position, exitPaths[i].rotation);
            obj.SetActive(true);
            exitPathGameObjects[i] = obj;
        }
    }

    public void DisableThisPath()
    {
        for (int i = 0; i < exitPathGameObjects.Length; i++)
            exitPathGameObjects[i].GetComponentInChildren<RunningPath>().Disable();

        if (PathGenerator.instance != null) PathGenerator.instance.AddPath(this.transform.parent.gameObject);
    }

    public void Disable()
    {
        if (PathGenerator.instance != null && !isPlayerInside) PathGenerator.instance.AddPath(this.transform.parent.gameObject);
    }
}
