using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggVisualIndicator : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    [SerializeField] Sprite[] m_EggSprites;

    int m_EggHP, m_EggMaxHP;
    int m_CurrentActiveSprite;

    List<int> m_EggDivisions = new List<int>();

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_EggHP = GameController.MainTowerHP;
        m_EggMaxHP = GameController.MainTowerHP;

        m_EggDivisions.Add(0);

        for (int i = 0; i < m_EggSprites.Length; i++)
        {
            if (i != m_EggSprites.Length - 1)
            {
                m_EggDivisions.Add((m_EggMaxHP / (m_EggSprites.Length) * i) + (m_EggMaxHP / (m_EggSprites.Length)));
            }
            else
            {
                m_EggDivisions.Add(m_EggMaxHP);
            }
        }
    }

    void FixedUpdate()
    {
        m_EggHP = GameController.MainTowerHP;

        for (int i = 0; i < m_EggDivisions.Count - 1; i++)
        {
            if (m_CurrentActiveSprite != i)
            {
                if (m_EggHP > m_EggDivisions[i] && m_EggHP <= m_EggDivisions[i + 1])
                {
                    print(m_EggDivisions[i]);
                    m_SpriteRenderer.sprite = m_EggSprites[i];
                    m_CurrentActiveSprite = i;
                }
            }
        }
    }
}