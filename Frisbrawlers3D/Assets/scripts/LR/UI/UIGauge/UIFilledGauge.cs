using UnityEngine;
using System.Collections;

public class UIFilledGauge : UIAbstractGauge
{
    [SerializeField] float m_maxFill = 1;

    override protected void SetValue_protected(float _value)
    {
        base.SetValue_protected(_value);
        m_gaugeImage.fillAmount = _value * m_maxFill;
    }
}
