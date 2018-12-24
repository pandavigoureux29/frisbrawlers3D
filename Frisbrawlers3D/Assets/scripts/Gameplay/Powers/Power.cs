using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Power : MonoBehaviour {

    protected Player player;
    protected Frisbee frisbee;

    public float Cooldown = 5;
    public float EffectTime = 3;

    protected bool active = false;

    protected DateTime cooldownDeadline;

    public bool IsReady {  get { return DateTime.UtcNow >= cooldownDeadline && !active; } }
    public double RemainingCooldown {  get { return (cooldownDeadline - DateTime.UtcNow).TotalSeconds; } }

    public event EventHandler<EventArgs> OnPowerActivate;
    public event EventHandler<EventArgs> OnPowerStop;

    // Use this for initialization
    protected virtual void Start () {
        player = GetComponent<Player>();
        frisbee = FindObjectOfType<Frisbee>();
        frisbee.OnCaught += Frisbee_OnCaught;
        frisbee.OnLaunched += Frisbee_OnLaunched;

        var matchManager = FindObjectOfType<GameMatchManager>();
        matchManager.OnGoal += MatchManager_OnGoal;
    }

    protected virtual void Frisbee_OnLaunched(object sender, EventArgs e)
    {
    }

    protected virtual void MatchManager_OnGoal(object sender, EventArgs e)
    {
        if (active)
            Deactivate();
    }

    protected virtual void Frisbee_OnCaught(object sender, EventArgs e)
    {
        if (active)
            Deactivate();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public virtual bool Activate()
    {
        if (!IsReady)
            return false;
        active = true;
        OnPowerActivate?.Invoke(this, null);

        return true;
    }

    public virtual void Deactivate()
    {
        active = false;
        OnPowerStop?.Invoke(this, null);
    }

    public void CheckActivable()
    {

    }
}
