using UnityEngine;
using System.Collections;

public class AlphaSetter : MonoBehaviour {

    [SerializeField] protected float Alpha = 1.0f;
    private SpriteRenderer m_renderer;

    void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color color = m_renderer.color;
        color.a = Alpha;
        m_renderer.color = color;
    }
}
