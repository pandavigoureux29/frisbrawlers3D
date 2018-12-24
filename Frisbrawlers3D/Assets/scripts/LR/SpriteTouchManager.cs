using UnityEngine;
using System.Collections;

/// <summary>
/// When the input is manipulated, treats the info and check the first collider touched in the scene.
/// You can narrow the colliders by setting a tag.
/// It is not recommended to have more than one insance in a scene.
/// </summary>
public class SpriteTouchManager : MonoBehaviour {

    [SerializeField] public string m_touchableTag = null;
    [SerializeField] float m_slideMaxTime = 0.0f;
    [SerializeField] float m_slideMinLength = 0.5f;

    protected bool m_pressed = false;
    Collider2D m_pressedCollider = null;

    private Vector2 m_deltaSlide;
    private Vector2 m_positionPressed;
    private float m_timePressed;
    private bool m_sliding;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        ProcessTouch();
    }

    protected void ProcessTouch() {

        //Check press
        if (m_pressed == false && IsJustPressed())
        {
            m_pressed = true;
            //compute position pressed
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(CustomGetTouchPosition());
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            m_positionPressed = touchPos;
            m_pressedCollider = Physics2D.OverlapPoint(touchPos);
            m_timePressed = 0;
            OnPressed(m_pressedCollider);
        }

        // check slide and release
        if (m_pressed)
        {
            m_timePressed += Time.deltaTime;
            //compute position pressed
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(CustomGetTouchPosition());
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            var currentCollider = Physics2D.OverlapPoint(touchPos);

            //compute delta vector from first press point to actual pressed point
            m_deltaSlide = touchPos - m_positionPressed;
            //if just released
            if (IsJustReleased())
            {
                m_pressed = false;
                if (m_sliding)
                {
                    //slide end
                    OnSlidReleased(m_pressedCollider, currentCollider);
                    m_pressedCollider = null;
                }
                else
                {
                    if( m_pressedCollider == currentCollider)
                        OnReleased(m_pressedCollider);
                    m_pressedCollider = null;
                }
                m_sliding = false;
            }
            else
            {
                //already sliding
                if( m_sliding)
                {
                    OnSliding(m_pressedCollider, currentCollider);
                }
                else
                {
                    //no slide but Still pressing => check slide
                    if ((m_timePressed <= m_slideMaxTime || m_slideMaxTime == 0.0f) && !m_sliding && m_deltaSlide.magnitude >= m_slideMinLength)
                    {
                        m_sliding = true;
                    }
                }
            }
        }
    }

    protected virtual void OnPressed(Collider2D _collider) { }

    protected virtual void OnReleased(Collider2D _collider) { }

    protected virtual void OnSlidReleased(Collider2D _startCollider, Collider2D _endCollider) { }

    protected virtual void OnSliding(Collider2D _startCollider, Collider2D _endCollider) { }

    Vector2 CustomGetTouchPosition()
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        return Input.mousePosition;
#else
		return Input.GetTouch(0).position;
#endif
    }

    bool IsJustPressed()
    {
#if UNITY_ANDROID
        bool b = false;
        for (int i = 0; i < Input.touches.Length; i++)
        {
            b = Input.touches[i].phase == TouchPhase.Began;
            if (b)
                break;
        }
        return b;
#endif
		return Input.GetMouseButtonDown(0);
    }

    bool IsJustReleased()
    {
#if UNITY_ANDROID
        bool b = false;
        for (int i = 0; i < Input.touches.Length; i++)
        {
            b = Input.touches[i].phase == TouchPhase.Ended;
            if (b)
                break;
        }
        return b;        
#endif
		return Input.GetMouseButtonUp(0);
    }

    protected Collider2D PressedCollider
    {
        get { return m_pressedCollider; }
    }
}
