using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteTouchHandler : MonoBehaviour {

    [SerializeField] List<GameObject> m_targetsCallback;

    [SerializeField] Collider2D m_collider;

    [SerializeField] string m_onInputDownCallback;
    [SerializeField] string m_onInputUpCallback;

    bool m_pressed = false;

    // Use this for initialization
    void Start()
    {
        if (m_collider == null)
            m_collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check press
        if (m_pressed == false && IsPressed())
        {
            SpriteTouchData touchData = ComputeTouch();
            //check attack 
            if (m_collider == touchData.colliderTouched)
            {
                m_pressed = true;
                SendCallbacks(m_onInputDownCallback,touchData);
            }
        }

        //Check Release
        if (m_pressed && IsReleased())
        {
            m_pressed = false;
            SpriteTouchData touchData = ComputeTouch();
            if (m_collider == touchData.colliderTouched)
            {
                SendCallbacks(m_onInputUpCallback,touchData);
            }
        }
    }

    SpriteTouchData ComputeTouch()
    {
        SpriteTouchData touchData = new SpriteTouchData();
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(CustomGetTouchPosition());
        touchData.touchPosition = new Vector2(worldPoint.x, worldPoint.y);
        touchData.colliderTouched = Physics2D.OverlapPoint(touchData.touchPosition);
        touchData.sourceComponent = this;
        return touchData;
    }

    Vector2 CustomGetTouchPosition()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        return Input.mousePosition;
#else
		return Input.GetTouch(0).position;
#endif
    }

    bool IsPressed()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        return Input.GetMouseButtonDown(0);
#else
			return Input.touchCount == 1;
#endif
    }

    bool IsReleased()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        return Input.GetMouseButtonUp(0);
#else
	return Input.touchCount < 1 ;
#endif
    }

    void SendCallbacks(string callback, SpriteTouchData data)
    {
        foreach( var go in m_targetsCallback)
        {
            go.SendMessage(callback, data, SendMessageOptions.DontRequireReceiver);
        }
    }

    public class SpriteTouchData
    {
        public Vector2 touchPosition;
        public Collider2D colliderTouched;
        public Component sourceComponent;        
    }
}
