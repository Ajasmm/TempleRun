using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlace : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) Invoke("DestroyThis", 3F);

        GamePlayManager.instance.StartFollow();

    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
