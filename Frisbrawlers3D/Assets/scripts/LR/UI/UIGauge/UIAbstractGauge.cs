using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class UIAbstractGauge : MonoBehaviour {

    [SerializeField]  protected float m_currentValue = 1.0f;

    [SerializeField]  protected Image m_gaugeImage;

    protected Transform m_gaugeTransform;

    protected float m_time;

    //FILLING
    /// <summary>
    /// This means the rate of the filling
    /// </summary>
    [SerializeField]
    private float m_scaleFillUnitValue = 0.01f;

    protected bool m_isFilling = false;

    protected float m_targetFillScale = 1.0f;

    protected float m_timeByScaleUnit = 0.01f;

    protected int m_fillCount = 0;
    protected int m_fillCountTarget = 0;


    // Use this for initialization
    virtual protected void Awake()
    {
        if (m_gaugeImage != null)
        {
            m_gaugeTransform = m_gaugeImage.transform;
        }
        SetValue(m_currentValue);
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

                //set scale
                SetValue_protected(m_currentValue);

                //check if ended
                if (m_currentValue >= m_targetFillScale && m_fillCount >= m_fillCountTarget)
                {
                    m_currentValue = m_targetFillScale;
                    m_isFilling = false;
                }

                //Fill count 
                if (m_currentValue >= 1.0f && m_fillCount < m_fillCountTarget)
                {
                    m_fillCount++;
                    m_currentValue = 0.0f;
                }
            }
        }
    }

    /// <summary>
    /// Set value between 0 and 1
    /// </summary>
    virtual public void SetValue(float _value, bool _fill = false, float _fillDuration = 1.0f, int _fillCount = 0)
    {
        if (m_gaugeTransform == null)
        {
            return;
        }

        //Cap values
        if (_value < 0.0f) _value = 0.0f;
        if (_value > 1.0f) _value = 1.0f;

        m_isFilling = _fill;

        if (_fill)
        {
            //count
            m_fillCount = 0;
            m_fillCountTarget = _fillCount;

            m_targetFillScale = _value;
            //Get direction of the scroll
            float delta = Mathf.Abs(_value - m_currentValue);
            //compute the speed
            m_timeByScaleUnit = _fillDuration * m_scaleFillUnitValue / Mathf.Abs(delta);
        }
        else
        {
            m_currentValue = _value;
            SetValue_protected(_value);
        }

    }

    virtual protected void SetValue_protected(float _value)
    {
        m_currentValue = _value;
    }

}
