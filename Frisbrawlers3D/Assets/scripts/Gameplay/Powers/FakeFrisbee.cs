using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class FakeFrisbee : Frisbee {

    float maxBounces = 1;
    float bounces = 0;
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }

    public override bool SetPlayer(Player attachedPlayer)
    {
        lastPlayer = attachedPlayer;
        Deactivate();
        return false;
    }

    public override void Bounce(Vector2 bouncingScale)
    {
        base.Bounce(bouncingScale);
        bounces++;
        if(bounces > maxBounces)
        {
            Deactivate();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        bounces = 0;
    }
}
