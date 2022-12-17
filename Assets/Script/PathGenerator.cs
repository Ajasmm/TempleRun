using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public static PathGenerator instance;

    [SerializeField] List<GameObject> straightPathPrefabs, turnPathPrefabs;

    Queue<GameObject> straightPaths, turnPaths;

    int count = 0;

    private void OnEnable()
    {
        if(instance == null) {
            instance = this;
            transform.parent = null;
        }else Destroy(gameObject);



        straightPaths = new Queue<GameObject>();
        turnPaths = new Queue<GameObject>();

        for (int i = 0; i < straightPathPrefabs.Count; i++)
        {
            GameObject obj = Instantiate(straightPathPrefabs[i], transform);
            AddToStraightPath(obj.GetComponentInChildren<RunningPath>().gameObject);
        }
        for (int i = 0; i < straightPathPrefabs.Count; i++)
        {
            GameObject obj = Instantiate(straightPathPrefabs[i], transform);
            AddToStraightPath(obj.GetComponentInChildren<RunningPath>().gameObject);
        }

        for (int i = 0; i < turnPathPrefabs.Count; i++)
        {
            GameObject obj = Instantiate(turnPathPrefabs[i % straightPaths.Count], transform);
            AddToTurnPath(obj.GetComponentInChildren<RunningPath>().gameObject);
        }

        GameObject firstObject = GetPath();
        firstObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        firstObject.SetActive(true);
        
    }

    private void AddToStraightPath(GameObject path)
    {
        GameObject parent = path.transform.parent.gameObject;

        if (parent == null)
        {
            path.SetActive(false);
            straightPaths.Enqueue(path);
        }
        else
        {
            parent.SetActive(false);
            straightPaths.Enqueue(parent);
        }
    }

    private GameObject GetStraightPath()
    {
        GameObject obj;
        obj = straightPaths.Dequeue();

        return obj;
    }

    private void AddToTurnPath(GameObject path)
    {
        GameObject parent = path.transform.parent.gameObject;

        if (parent == null)
        {
            path.SetActive(false);
            turnPaths.Enqueue(path);
        }
        else
        {
            parent.SetActive(false);
            turnPaths.Enqueue(parent);
        }
    }

    private GameObject GetTurnPath()
    {
        GameObject obj;
        obj = turnPaths.Dequeue();

        return obj;
    }

    public GameObject GetPath()
    {
        count++;
        GameObject path;

        if (count % 3 == 0) path = GetTurnPath();
        else path = GetStraightPath();

        if(path == null)
        {
            if (straightPaths.Count > 0) path = GetStraightPath();
            else if (turnPaths.Count > 0) path = GetTurnPath();
            else path = null;
        }

        return path;
    }
    public void AddPath(GameObject parent)
    {
        RunningPath path = parent.GetComponentInChildren<RunningPath>();
        if (path == null) return;

        if (path.type == PathType.Straight) AddToStraightPath(path.gameObject);
        else if (path.type == PathType.Turning) AddToTurnPath(path.gameObject);
    }
}
