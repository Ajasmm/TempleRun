using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] MeshRenderer m_Renderer;

    private void OnEnable()
    {
        m_Renderer.enabled = true;
    }

    public void Destroy()
    {
        GamePlayManager.instance.IncreaseScore();
        m_Renderer.enabled = false;
    }
}
