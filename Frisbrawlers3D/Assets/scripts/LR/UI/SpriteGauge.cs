using UnityEngine;
using System.Collections;

public class SpriteGauge : MonoBehaviour {

	[SerializeField] protected float m_currentValue = 1.0f;
    
	[SerializeField] protected LR.UI.ORIENTATION m_orientation;
    
	[SerializeField] protected LR.UI.ALIGN m_align;

	[SerializeField] protected SpriteRenderer m_gaugeSpr;

	[SerializeField] protected float m_maxScale = 1.0f;
	[SerializeField] protected bool m_useSprScaleAsMax = true;
    
	private Vector3 m_initPos;

    private float m_posAtZero = 0;
    private float m_posRange = 0;

	protected Transform m_gaugeTransform;

    private float m_time;

    private bool m_isFilling = false;

    private float m_targetFillScale = 1.0f;

    private float m_timeByScaleUnit = 0.01f;

    private int m_fillCount = 0;
    private int m_fillCountTarget = 0;

    /// <summary>
    /// This means the rate of the filling
    /// </summary>
    [SerializeField]  private float m_scaleFillUnitValue = 0.01f; 

	// Use this for initialization
	virtual protected void Awake () {

        if (m_gaugeSpr != null)
        {
            m_gaugeTransform = m_gaugeSpr.transform;

            //Override max scale if needed
            if (m_useSprScaleAsMax)
            {
                m_maxScale = m_orientation == LR.UI.ORIENTATION.HORIZONTAL ? m_gaugeTransform.localScale.x : m_gaugeTransform.localScale.y;
            }
            ComputeEdge();
        }
        
		SetValue (m_currentValue);
	}

    protected virtual void Update()
    {
        if (m_isFilling)
        {
            m_time += Time.deltaTime;
            if (m_time >= m_timeByScaleUnit)
            {
                m_time = 0;
                //increase value
                m_currentValue += m_scaleFillUnitValue;

                //Compute and set scale/position
                float newScale = m_currentValue * m_maxScale;
                float newPos = m_posAtZero + m_posRange * m_currentValue;
                SetValue_private(newScale, newPos);

                //check if ended
                if (m_currentValue >= m_targetFillScale && m_fillCount >= m_fillCountTarget)
                {
                    m_currentValue = m_targetFillScale;
                    m_isFilling = false;
                }

                //Fill count 
                if(m_currentValue >= 1.0f && m_fillCount < m_fillCountTarget)
                {
                    m_fillCount++;
                    m_currentValue = 0.0f;
                }
            }
        }
    }

    public void ChangeOrientation(LR.UI.ORIENTATION _orientation, LR.UI.ALIGN _align)
    {
        m_orientation = _orientation;
        m_align = _align;
        m_gaugeTransform.localPosition = m_initPos;
        ComputeEdge();
    }


    /// <summary>
    /// Scale the sprite to max value, then compute the edge according to the orientation and alignement
    /// The edge is the position of the sprite when the gauge is at 0
    /// </summary>
    void ComputeEdge()
    {
        float posAtMax = 0.0f;
        //change scale to max to have the entire gauge full
        if (m_orientation == LR.UI.ORIENTATION.HORIZONTAL)
        {
            Utils.SetLocalScaleX(m_gaugeTransform, m_maxScale);
            var worldToLocal = m_gaugeTransform.localPosition - m_gaugeTransform.position;
            if ( m_align == LR.UI.ALIGN.LEFT)
            {
                m_posAtZero = m_gaugeSpr.bounds.min.x + worldToLocal.x;
            }else if(m_align == LR.UI.ALIGN.RIGHT)
            {
                m_posAtZero = m_gaugeSpr.bounds.max.x + worldToLocal.x;
            }
            posAtMax = m_gaugeSpr.bounds.center.x + worldToLocal.x;
        }
        else
        {
            Utils.SetLocalScaleY(m_gaugeTransform, m_maxScale);
            var worldToLocal = m_gaugeTransform.localPosition - m_gaugeTransform.position;
            if (m_align == LR.UI.ALIGN.BOTTOM)
            {
                m_posAtZero = m_gaugeSpr.bounds.min.y + worldToLocal.y;
            }
            else if (m_align == LR.UI.ALIGN.TOP)
            {
                m_posAtZero = m_gaugeSpr.bounds.max.y + worldToLocal.y;
            }
            posAtMax = m_gaugeSpr.bounds.center.y + worldToLocal.y;
        }
        //Range from minimum position to max ( length of the demi gauge )
        m_posRange = posAtMax - m_posAtZero;
        m_initPos = m_gaugeTransform.localPosition;
    }
    
    /// <summary>
    /// Set value between 0 and 1
    /// </summary>
	virtual public void SetValue(float _value , bool _fill = false, float _fillDuration = 1.0f, int _fillCount = 0){

        if (m_gaugeTransform == null)
        {
            return;
        }

        //Cap values
		if (_value < 0.0f) _value = 0.0f;
		if (_value > 1.0f) _value = 1.0f;

        m_isFilling = _fill;

        if( _fill)
        {
            //count
            m_fillCount = 0;
            m_fillCountTarget = _fillCount;

            m_targetFillScale = _value;
            //Get direction of the scroll
            float delta = Mathf.Abs( _value - m_currentValue );
            //compute the speed
            m_timeByScaleUnit = _fillDuration * m_scaleFillUnitValue / Mathf.Abs(delta);
        }
        else
        {
            m_currentValue = _value;

            float newScale = _value * m_maxScale;
            float newPos = m_posAtZero + m_posRange * m_currentValue;

            SetValue_private(newScale, newPos);
        }

    }

    void SetValue_private(float _scaleValue, float _posValue)
    {
        //change scale
        if (m_orientation == LR.UI.ORIENTATION.HORIZONTAL)
        {
            Utils.SetLocalScaleX(m_gaugeTransform, _scaleValue);
            Utils.SetLocalPositionX(m_gaugeTransform, _posValue);
        }
        else
        {
            Utils.SetLocalScaleY(m_gaugeTransform, _scaleValue);
            Utils.SetLocalPositionY(m_gaugeTransform, _posValue);
        }
    }
    
}
