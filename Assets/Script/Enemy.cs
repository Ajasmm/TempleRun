using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform player;
    [SerializeField] Vector3 normalOffset;

    bool isPlayerDead = false;
    float height = 0F;
    Vector3 pos;

    int winTrigger;

    Transform myTransform;

    private void OnEnable()
    {
        myTransform = transform;
        GamePlayManager.instance.OnGameOver += OnPlayerDead;
    }

    private void OnDisable()
    {
        GamePlayManager.instance.OnGameOver -= OnPlayerDead;
    }

    private void Start()
    {
        winTrigger = Animator.StringToHash("Won");
    }

    public void Update()
    {
        if (isPlayerDead) return;

        pos = player.TransformPoint(normalOffset);
        pos.y = 0f;
        myTransform.SetPositionAndRotation(pos, player.rotation);
    }



    public void OnPlayerDead()
    {
        isPlayerDead = true;
        animator.SetTrigger(winTrigger);
    }


}
