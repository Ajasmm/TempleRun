using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RunningPath : MonoBehaviour
{
    [SerializeField] public PathType type = PathType.Straight;
    [SerializeField] Transform[] exitPaths;

    GameObject[] exitPathGameObjects;

    private void Awake()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) InstanciateExitPaths();
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")) Invoke("DisableThisPath", 2F);
    }

    private void InstanciateExitPaths()
    {
        exitPathGameObjects = new GameObject[exitPaths.Length];
        foreach(Transform t in exitPaths)
        {
            GameObject obj = PathGenerator.instance.GetPath();
            obj.transform.SetPositionAndRotation(t.position, t.rotation);
            obj.SetActive(true);
        }
    }

    public void DisableThisPath()
    {
        if (PathGenerator.instance != null) PathGenerator.instance.AddPath(this.transform.parent.gameObject);
    }
}
