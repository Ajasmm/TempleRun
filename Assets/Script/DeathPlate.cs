using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeathPlate : MonoBehaviour
{
    [SerializeField] Transform player;
    Rigidbody rBody;

    Vector3 tempPos;

    private void Awake()
    {
        rBody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(player == null) this.enabled = false;
    }

    private void FixedUpdate()
    {
        tempPos = player.position;
        tempPos.y = -0.5F;
        rBody.MovePosition(tempPos);
    }

}
