using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class UISequence : MonoBehaviour {

    [SerializeField] public List<UIStep> m_steps;

    protected int m_currentStepIndex = 0;

    private bool m_started = false;

    public delegate void OnSeqenceEndDelegate(UISequence sequence);
    public OnSeqenceEndDelegate m_endDelegate;

    void Awake()
    {
        m_steps = GetComponentsInChildren<UIStep>().ToList();
    }
    
    /// <summary>
    /// If autoLaunch, the first step is launched directly.
    /// delegate : public void OnSeqenceEndDelegate(UISequence sequence)
    /// </summary>
    public virtual void Launch (OnSeqenceEndDelegate _del, bool _autoLaunch = true) {
        if (m_steps.Count <= 0)
            return;

        HookSkipOnTouch();

        m_endDelegate = _del;
        m_currentStepIndex = 0;
        m_started = true;
        if(_autoLaunch)
            LaunchNextStep();
	}

    protected void LaunchNextStep()
    {
        //launch as many consecutive non blocking steps as they are
        for (int i = m_currentStepIndex; i < m_steps.Count; i++)
        {
            m_currentStepIndex = i;
            LaunchStep();
            if (CurrentStep.IsBlocking)
                break;
        }
    }

    protected virtual void LaunchStep()
    {
        CurrentStep.Launch(OnStepEnd);
    }

    protected virtual void Update () {
	}

    public virtual void Skip()
    {
        if(CurrentStep != null)
            CurrentStep.Skip();
    }

    public virtual void Stop()
    {
        UnhookSkipOnTouch();
        m_started = false;
        if(m_endDelegate != null)
            m_endDelegate.Invoke(this);
    }

    public virtual void OnStepEnd(UIStep step)
    {
        if (!step.IsBlocking)
            return;

        if( m_currentStepIndex < m_steps.Count - 1)
        {
            m_currentStepIndex++;
            LaunchNextStep();
        }else
        {
            Stop();
        }
    }

    public bool IsStarted { get { return m_started; } }

    protected UIStep CurrentStep {
        get {
            if (m_currentStepIndex >= m_steps.Count )
                return null;
            return m_steps[m_currentStepIndex];
        }
    }

    protected void HookSkipOnTouch()
    {
        //InputManager.instance.OnMousePressedDown += OnSkip;
    }

    protected void UnhookSkipOnTouch()
    {
        //InputManager.instance.OnMousePressedDown -= OnSkip;
    }

    protected void OnSkip(object sender, EventArgs args)
    {
        if (m_started)
            Skip();
    }
}
