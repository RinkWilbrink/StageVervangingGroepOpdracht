using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSortingLayer : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    int m_Accuracy = 10;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        int transformZ = (int)(transform.position.z * m_Accuracy);
        int transformX = (int)(transform.position.x * m_Accuracy);
        transformZ = transformZ * m_Accuracy;

        int spriteOrderNumber = (transformX + transformZ) * -1;
        m_SpriteRenderer.sortingOrder = spriteOrderNumber;
    }
}
