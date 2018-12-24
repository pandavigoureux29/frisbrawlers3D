using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITextNumberScroller : MonoBehaviour {

    [SerializeField] float m_defaultScrollTime = 5.0f;
    protected Text m_text;

    protected bool m_scrolling = false;

    protected int m_targetNumber = 0;

    protected float m_stepTime = 0.05f;
    protected float m_unitsByStep = 1;
    protected int m_direction = 1;

    protected float m_time = 0;
    protected float m_current = 0;

    public delegate void OnTargetReached(UITextNumberScroller scroller);
    OnTargetReached m_callback;
    
    // Use this for initialization
    void Awake () {
        m_text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	    if( m_scrolling )
        {
            m_time += Time.deltaTime;
            if( m_time >= m_stepTime)
            {
                m_time = 0;
                m_current += m_direction * m_unitsByStep;

                bool targetReached = m_direction > 0 ? m_current >= m_targetNumber : m_current <= m_targetNumber;
                if (targetReached)
                {
                    m_current = m_targetNumber;
                    TargetReached();
                }
                m_text.text = "" + ((int)Mathf.Round(m_current));
            }
        }
	}
    
    protected virtual void TargetReached()
    {
        m_scrolling = false;
        if (m_callback != null)
            m_callback(this);
    }

    /// <summary>
    /// Scroll To the target. Delegate usage : public void OnTargetReached(UITextNumberScroller scroller)
    /// </summary>
    public virtual void ScrollTo(int _targetNumber, float _duration = -1.0f, OnTargetReached del = null)
    {
        m_targetNumber = _targetNumber;
        m_scrolling = true;

        m_current = float.Parse(m_text.text);
        ScrollFromTo( (int)m_current, _targetNumber, _duration,del);
    }

    /// <summary>
    /// Scroll from To the target. Delegate usage : public void OnTargetReached(UITextNumberScroller scroller)
    /// </summary>
    public virtual void ScrollFromTo(int _from, int _targetNumber, float _duration = -1.0f, OnTargetReached del = null)
    {
        m_targetNumber = _targetNumber;
        m_callback = del;
        m_scrolling = true;

        m_current = _from;
        m_text.text = "" + ((int)Mathf.Round(m_current));
        //Get direction of the scroll
        float delta = _targetNumber - m_current;
        m_direction = delta < 0 ? -1 : 1;
        if (_duration < 0)
            _duration = m_defaultScrollTime;
        float stepsInDuration = _duration / m_stepTime;
        //compute the speed
        m_unitsByStep = delta / stepsInDuration;
        m_time = 0;
    }

    public void Skip()
    {
        m_text.text = m_targetNumber.ToString();
        TargetReached();
    }

    public bool Scrolling
    {
        get
        {
            return m_scrolling;
        }        
    }
}
