using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningPaths : MonoBehaviour
{
    [SerializeField] Transform[] instancePoints;
    [SerializeField] PathType pathType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) Invoke("DisableThis", 1F);
    }

    private void DisableThis()
    {

    }
}

public enum PathType
{
    Turns,
    Straights
}
