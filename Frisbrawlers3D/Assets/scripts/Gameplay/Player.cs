using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] CharacterController controller;

    public float m_acceleration = 3;
	public float m_rotationAcceleration = 10.0f;

	public float m_maxSpeed = 10;

    public float Speed = 6.0f;

    public float FastLaunchDelayMax = 0.8f;
    System.DateTime catchTime;

    Frisbee frisbee;
    Vector3 lastDirection = new Vector3(1, 0, 0);
    Vector3 facingDirection = new Vector3(1, 0, 0);

	public int TeamId = 0;

    public bool IsReversed = false;
    float reverseMult { get { return IsReversed ? -1 : 1; } }

    public bool HasFrisbee { get { return frisbee != null; } }

    public bool CurveActive = false;

    [SerializeField] Rigidbody m_rigidbody;
    [SerializeField] GameObject spriteObject;

	public Vector2 forwardVectorBase = new Vector2(1,0);
	Vector2 m_forwardVector;

	PlayerCamera m_camera;

    public enum State { NONE, CAUGHT_FRISBEE, LAUNCHING }
    public State CurrentState = State.NONE;

    // Use this for initialization
    void Start () {
		m_forwardVector = forwardVectorBase;
        var sprite = transform.GetComponentInChildren<SpriteRenderer>();

		m_camera = FindObjectOfType<PlayerCamera> ();
		if (m_camera != null)
			m_camera.Player = this.transform;
	}

	// Update is called once per frame
	void Update () {

        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;

		//Debug.DrawLine (transform.position, transform.position + new Vector3 (m_forwardVector.x, m_forwardVector.y, 0) * 3, Color.red, 0.01f, true);
	}
    
	public void SetTeam(int _teamId){
		TeamId = _teamId;
	}

	void OnChangeTeamId(int _teamId){
		if (_teamId == 1)
        {
            Reverse();
            if (m_camera != null)
				m_camera.Reverse ();
		}
	}

	public void Reverse()
    {
        RotateBody(180);
        IsReversed = true;
    }

    #region Launch

    public void OnCatchFrisbee(Frisbee _frisbee)
    {
        if (_frisbee.IsFree && CurrentState != State.LAUNCHING)
        {
            bool success = _frisbee.SetPlayer(this);
            if (success)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                frisbee = _frisbee;
                CurrentState = State.CAUGHT_FRISBEE;
                catchTime = System.DateTime.UtcNow;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.layer == LayerMask.NameToLayer("Frisbee"))
        {
            Frisbee collidingFrisbee = other.gameObject.GetComponent<Frisbee>();
            OnCatchFrisbee(collidingFrisbee);
        }*/
    }


    public void OnLaunch(float x, float z)
    {
        Launch(x, z);
    }


    void Launch(float x, float z)
    {
        if (frisbee != null)
        {
            //fast launch
            var launchdelay = (System.DateTime.UtcNow - catchTime).TotalSeconds;
            bool fastLaunch = launchdelay <= FastLaunchDelayMax;

            var launchVector = new Vector3(x * reverseMult, 0, z * reverseMult);
            frisbee.Launch(launchVector, CurveActive, fastLaunch);
            frisbee = null;
            CurveActive = false;
            CurrentState = State.LAUNCHING;
            StartCoroutine(WaitAfterLaunch());
        }
    }

    IEnumerator WaitAfterLaunch()
    {
        yield return new WaitForSeconds(0.5f);
        CurrentState = State.NONE;
    }

    #endregion

    #region POWER

    public void OnPowerActivate()
    {
        GetComponent<Power>().Activate();
    }

    public void OnToggleCurve()
    {
        CurveActive = !CurveActive;
    }
    #endregion

    #region MOVEMENT

    public void OnMove(Vector3 moveVector)
    {
        moveVector = moveVector.normalized;
        Move(moveVector.x, moveVector.z);
    }

    public void OnMove(Vector2 moveVector)
    {
        moveVector = moveVector.normalized;
        Move(moveVector.x, moveVector.y);
    }

    void Move(float x, float z)
    {
        if ( (x != 0 || z != 0) && CurrentState == State.NONE)
        {
            Vector3 moveVector = new Vector3(x, 0, z).normalized;
            moveVector *= Speed * reverseMult;
            if (frisbee == null)
            {
                AddSpeed(moveVector);
                lastDirection = moveVector;
            }
            facingDirection = moveVector;
        }
    }

    void AddSpeed(Vector3 moveVector)
    {
        controller.Move(moveVector);

        /*var rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(moveVector);
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, m_maxSpeed);*/
    }

    #endregion

    #region MOVEMENTS_OBSOLETE

    public void MoveForward(){
		m_rigidbody.AddForce ( m_forwardVector * m_acceleration );
	}

	public void RotateRight(){
		float angle = m_rotationAcceleration * Time.fixedDeltaTime;
		angle = NormalizeAngle (angle);
		RotateBody (angle);
	}

	public void RotateLeft(){
		float angle = -m_rotationAcceleration * Time.fixedDeltaTime;
		angle = NormalizeAngle (angle);
		RotateBody (angle);
	}

	void RotateBody(float angle){
        spriteObject.transform.Rotate(new Vector3(0, 0, 1), 180);
	}

	public Vector2 Rotate(Vector2 v, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (sin * tx) + (cos * ty);
		return v;
	}

	//Normalizes angle between start and end
	float NormalizeAngle(float value){
		float width = 360 - 0;
		float offsetValue = value - 0;
		return (offsetValue - (Mathf.Floor (offsetValue / width) * width)) ;
	}
       

    #endregion

}
