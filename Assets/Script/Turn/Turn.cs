using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    [SerializeField] List<Direction> directions;
    [SerializeField] Vector3 centerPos;


    private void OnEnable()
    {
        Debug.Log("Turn Enable Called");
        centerPos = transform.position;
    }
    public List<Direction> GetDirections() { return directions; }
    public Vector3 GetCenterPos() { return centerPos; }
}
