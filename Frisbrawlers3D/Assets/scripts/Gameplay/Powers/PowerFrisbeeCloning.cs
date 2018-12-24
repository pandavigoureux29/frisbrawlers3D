using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class PowerFrisbeeCloning : Power {

    [SerializeField] GameObject FrisbeeClonePrefab;
    FakeFrisbee frisbeeClone;

	// Use this for initialization
	protected override void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void CreateClone()
    {
        if (frisbeeClone != null)
            return;
        var frisbeeGo = GameObject.Instantiate(FrisbeeClonePrefab) as GameObject;
        NetworkServer.Spawn(frisbeeGo);
        frisbeeClone = frisbeeGo.gameObject.GetComponent<FakeFrisbee>();
        frisbeeClone.gameObject.SetActive(false);
    }

    public override bool Activate()
    {
        if (!IsReady || !player.HasFrisbee)
            return false;

        if (frisbeeClone == null)
            CreateClone();

        base.Activate();        
        return true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        frisbeeClone.Deactivate();
    }

    protected override void Frisbee_OnLaunched(object sender, EventArgs e)
    {
        base.Frisbee_OnLaunched(sender, e);
        if (active)
        {
            cooldownDeadline = DateTime.UtcNow.AddSeconds(Cooldown);

            var realFrisbee = sender as Frisbee;

            frisbeeClone.gameObject.SetActive(true);
            frisbeeClone.transform.position = realFrisbee.transform.position;
            frisbeeClone.currentSpeed = realFrisbee.currentSpeed;
            var fakeDirection = GetFakeFrisbeeDirection(realFrisbee);
            frisbeeClone.Launch(fakeDirection, false, false);
        }
    }

    Vector2 GetFakeFrisbeeDirection(Frisbee frisbee)
    {

        Vector2 v = new Vector2(0.5f,0.5f);
        if(frisbee.direction.y > 0)
        {
            v = Utils.Rotate(frisbee.direction, 35);
        }
        else
        {
            v = Utils.Rotate(frisbee.direction, -35);
        }
        return v;
    }
}
