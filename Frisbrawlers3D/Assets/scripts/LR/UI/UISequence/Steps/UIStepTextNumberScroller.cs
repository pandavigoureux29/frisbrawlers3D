using UnityEngine;
using System.Collections;

public class UIStepTextNumberScroller : UIStep {

    [SerializeField] UITextNumberScroller m_text;

    [SerializeField] float m_scrollDuration = 0.0f;

    public override void Skip()
    {
        m_text.Skip();
    }

    public void Launch(OnStepEndDelegate _del, int _target)
    {
        base.Launch(_del);
        m_text.ScrollTo(_target, m_scrollDuration, OnTargetReached);
    }

    public void OnTargetReached(UITextNumberScroller scroller)
    {
        Stop();
    }

}
