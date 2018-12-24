using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIGauge : UIAbstractGauge {


    
    [SerializeField]
    protected LR.UI.ORIENTATION m_orientation;
    
    [SerializeField]
    protected LR.UI.ALIGN m_align;
    
    [SerializeField]
    protected float m_maxScale = 1.0f;
    [SerializeField]
    protected bool m_useSprScaleAsMax = true;

    private Vector3 m_initPos;

    private float m_posAtZero = 0;
    private float m_posRange = 0;
    
    // Use this for initialization
    override protected void Awake()
    {
        base.Awake();
        if (m_gaugeImage != null)
        {
            //Override max scale if needed
            if (m_useSprScaleAsMax)
            {
                m_maxScale = m_orientation == LR.UI.ORIENTATION.HORIZONTAL ? m_gaugeTransform.localScale.x : m_gaugeTransform.localScale.y;
            }
            SetPivot();
        }

        SetValue(m_currentValue);
    }

    public void ChangeOrientation(LR.UI.ORIENTATION _orientation, LR.UI.ALIGN _align)
    {
        m_orientation = _orientation;
        m_align = _align;
        m_gaugeTransform.localPosition = m_initPos;
        SetPivot();
    }

    /// <summary>
    /// Scale the sprite to max value, then compute the edge according to the orientation and alignement
    /// The edge is the position of the sprite when the gauge is at 0
    /// </summary>
    void SetPivot()
    {
        switch (m_orientation)
        {
            case LR.UI.ORIENTATION.HORIZONTAL:
                SetHorizontalPivot();
                break;
            case LR.UI.ORIENTATION.VERTICAL:
                SetVerticalPivot();
                break;
        }
        m_initPos = m_gaugeTransform.localPosition;
    }

    void SetHorizontalPivot()
    {
        RectTransform rT = m_gaugeImage.GetComponent<RectTransform>();
        switch (m_align)
        {
            case LR.UI.ALIGN.LEFT:
                rT.pivot = new Vector2(1, rT.pivot.y);
                break;
            case LR.UI.ALIGN.RIGHT:
                rT.pivot = new Vector2(0, rT.pivot.y);
                break;
            default:
                rT.pivot = new Vector2(0.5f, 0.5f);
                break;
        }
    }

    void SetVerticalPivot()
    {
        RectTransform rT = m_gaugeImage.GetComponent<RectTransform>();
        switch (m_align)
        {
            case LR.UI.ALIGN.TOP:
                rT.pivot = new Vector2(rT.pivot.x, 0);
                break;
            case LR.UI.ALIGN.BOTTOM:
                rT.pivot = new Vector2(rT.pivot.x, 1);
                break;
            default:
                rT.pivot = new Vector2(0.5f, 0.5f);
                break;
        }
    }
    
    override protected void SetValue_protected(float _value)
    {
        base.SetValue_protected(_value);
        float newScale = _value * m_maxScale;
        //change scale
        if (m_orientation == LR.UI.ORIENTATION.HORIZONTAL)
        {
            Utils.SetLocalScaleX(m_gaugeTransform, newScale);
        }
        else
        {
            Utils.SetLocalScaleY(m_gaugeTransform, newScale);
        }
    }
}
