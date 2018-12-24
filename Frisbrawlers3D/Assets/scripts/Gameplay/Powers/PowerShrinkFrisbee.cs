using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerShrinkFrisbee : Power {

    Vector3 frisbeeBaseScale;
    public float shrinkScaleFactor = 0.5f;

    void PreActivate()
    {
        
    }

    public override bool Activate()
    {
        if (!IsReady || !player.HasFrisbee)
            return false;
        base.Activate();

        frisbeeBaseScale = frisbee.transform.localScale;

        Vector3 shrinkScale = frisbee.transform.localScale * shrinkScaleFactor;
        frisbee.transform.localScale = shrinkScale;

        return true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        frisbee.transform.localScale = frisbeeBaseScale;
    }


    protected override void MatchManager_OnGoal(object sender, EventArgs e)
    {
        if (active)
            Deactivate();
    }

    protected override void Frisbee_OnCaught(object sender, EventArgs e)
    {
        if (active)
            Deactivate();
    }

    protected override void Frisbee_OnLaunched(object sender, EventArgs e)
    {
        base.Frisbee_OnLaunched(sender, e);
        if (active)
            cooldownDeadline = DateTime.UtcNow.AddSeconds(Cooldown);
    }
}
