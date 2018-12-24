using UnityEngine;
using System.Collections;

public abstract class UIStep : MonoBehaviour {

    [SerializeField] protected string m_id;

    [SerializeField] protected bool m_blocking = true;

    private bool m_started = false;

    public delegate void OnStepEndDelegate(UIStep sequence);
    public OnStepEndDelegate m_endDelegate;

    public virtual void Launch(OnStepEndDelegate _del)
    {
        m_started = true;
        m_endDelegate = _del;
    }
    
    void Update()
    {
        if (m_started)
        {
            UpdateStep();
        }
    }

    protected virtual void UpdateStep()
    {

    }

    public virtual void Skip()
    {
        Debug.Log("Skip");
        Stop();
    }
    
    protected virtual void Stop()
    {
        m_started = false;
        if (m_endDelegate != null)
            m_endDelegate.Invoke(this);
    }

    public bool IsBlocking { get { return m_blocking; } }
    public string Id { get { return m_id; } set { m_id = value; } }
}
