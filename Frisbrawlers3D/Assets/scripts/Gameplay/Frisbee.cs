﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Frisbee : MonoBehaviour {

    [SerializeField] float baseSpeed;
    [SerializeField] float maxSpeed;

    public float currentSpeed = 0;

    [SerializeField] float wallSpeedStep = 7;
    [SerializeField] float fastLaunchAddedSpeed = 5;

    [SerializeField] Transform spriteTransform;

    Player player;
    public Player lastPlayer;

    public Area currentArea;

    public Vector3 direction;
    Vector3 baseDirection;
    
    //CURVING
    bool Curved = false;
    float baseCurveAngle = 45;
    float curveAngle = 0;

    public float curveAngleDropRate = 1.3f;

    public enum State {  NONE, LAUNCHED, CAUGHT }
    public State CurrentState = State.NONE;

    public class FrisbeeEventArgs : EventArgs
    {
        public Player Player { get; set; }
        public Area Area { get; set; }

        public FrisbeeEventArgs(Player player, Area area)
        {
            Player = player;
            Area = area;
        }
    }

    public class FrisbeeGoalEventArgs : EventArgs
    {
        public Goal Goal { get; set; }

        public FrisbeeGoalEventArgs(Goal goal)
        {
            Goal = goal;
        }
    }

    public event EventHandler<EventArgs> OnWallBounce;
    public event EventHandler<FrisbeeEventArgs> OnCaught;
    public event EventHandler<FrisbeeEventArgs> OnLaunched;
    public event EventHandler<FrisbeeEventArgs> OnEnterArea;
    public event EventHandler<FrisbeeGoalEventArgs> OnGoal;

    public bool IsFree
    {
        get { return player == null; }
    }

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
    }

    void LateUpdate()
    {
        if (CurrentState == State.LAUNCHED)
        {
            float rotationSpeed = 220 * (Curved ? 1.5f : 1f);
            spriteTransform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);
        }
        else
        {

        }

        if (player != null)
        {
            transform.position = player.transform.position;            
        }
        else
        {
            Vector3 curvedV = ApplyCurve(direction);
            Vector3 v = curvedV * currentSpeed * Time.deltaTime;
            transform.Translate(v);
        }
        var pos = transform.position;
        pos.y = 0;
        transform.position = pos;
    }

    public virtual bool SetPlayer(Player attachedPlayer)
    {
        Curved = false;
        currentSpeed = baseSpeed;
        player = attachedPlayer;
        CurrentState = State.CAUGHT;
        OnCaught?.Invoke(this, new FrisbeeEventArgs(player,currentArea));
        return true;
    }

    public void Launch(Vector3 _direction, bool curved, bool fastLaunch)
    {
        lastPlayer = player;
        player = null;
        CurrentState = State.LAUNCHED;

        direction = _direction.normalized;
        baseDirection = _direction.normalized;
        
        if (curved)
        {
            curveAngle = baseCurveAngle;
            this.Curved = true;
        }

        if (fastLaunch)
        {
            currentSpeed += fastLaunchAddedSpeed;
        }

        OnLaunched?.Invoke(this, new FrisbeeEventArgs(lastPlayer, currentArea));
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player collidingPlayer = other.gameObject.GetComponent<Player>();
            if (player == collidingPlayer)
            {
                player = null;
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Area"))
        {
            currentArea = other.gameObject.GetComponent<Area>();
            OnEnterArea?.Invoke(this, new FrisbeeEventArgs(lastPlayer, currentArea));
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.OnCatchFrisbee(this);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Wall wall = other.gameObject.GetComponent<Wall>();
            Bounce(wall.BouncingVector);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Deadzone"))
        {
            FindObjectOfType<GameMatchManager>().Goal(false, 0);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Goal"))
        {
            var fake = gameObject.GetComponent<FakeFrisbee>();
            if (fake == null)
            {
                var goal = other.gameObject.GetComponent<Goal>();
                OnGoal?.Invoke(this, new FrisbeeGoalEventArgs(goal));
            }
        }
    }

    public virtual void Bounce(Vector3 bouncingScale)
    {
        direction.Scale( bouncingScale );
        currentSpeed += wallSpeedStep;
        if (currentSpeed > maxSpeed)
            currentSpeed = maxSpeed;
        Curved = false;
        OnWallBounce?.Invoke(this, null);
    }

    public void KickOff(bool right)
    {
        currentSpeed = baseSpeed * 0.5f;
        Vector3 moveVector = new Vector3(right ? 0.5f : -0.5f, 0, 0);
        Launch(moveVector,false,false);
    }

    public void ResetFrisbee()
    {
        direction = Vector3.zero;
        Curved = false;
    }

    #region Curve

    public Vector3 Rotate(Vector3 v, float degrees)
    {
        Quaternion rotation = Quaternion.Euler(0, degrees, 0);
        Vector3 myVector = Vector3.one;
        Vector3 rotateVector = rotation * myVector;

        return rotateVector;
    }

    Vector3 ApplyCurve(Vector3 direction)
    {
        if (!Curved)
            return direction;
        var v = Rotate(direction, curveAngle) ;
        curveAngle -= curveAngleDropRate;
        return v;
    }


    #endregion
}
