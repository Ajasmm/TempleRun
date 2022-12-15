using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    [SerializeField] List<Direction> directions;
    [SerializeField] float centerPos;

    public List<Direction> GetDirections() { return directions; }
    public float GetCenterPos() { return centerPos; }
}
