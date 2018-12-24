using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotAI : MonoBehaviour {

    public List<ShootTypeChance> ShootTypesList;
    public Vector2 ReactionTime;

    public enum BotState { WAITING_IDLE, WAITING_FOR_OPPONENT, MOVE_PREPARE_FOR_FRISBEE, MOVE_FOR_FRISBEE, LAUNCHING, RECENTER }
    [HideInInspector] public BotState State = BotState.WAITING_IDLE;
    
    Player player;
    Frisbee frisbee;

    List<Goal> opponentGoals;
    Area myArea;
    Vector2 shootDirection;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();

        frisbee = FindObjectOfType<Frisbee>();
        frisbee.OnCaught += Frisbee_OnCaught;
        frisbee.OnLaunched += Frisbee_OnLaunched;
        frisbee.OnWallBounce += Frisbee_OnWallBounce;
        frisbee.OnEnterArea += Frisbee_OnEnterArea;

        FindObjectOfType<GameMatchManager>().OnGoal += BotAI_OnGoal;

        player.IsReversed = false;

        opponentGoals = FindObjectsOfType<Goal>().Where(x => x.teamA == (player.TeamId != 0)).ToList();
        shootDirection = frisbee.transform.position.x < transform.position.x ? new Vector2(-1, 0) : new Vector2(1, 0);
        myArea = FindObjectsOfType<Area>().Where(x => x.TeamId == player.TeamId).FirstOrDefault();
	}

    private void BotAI_OnGoal(object sender, System.EventArgs e)
    {
        frisbee = GetFrisbee();
    }

    #region Event

    private void Frisbee_OnEnterArea(object sender, Frisbee.FrisbeeEventArgs e)
    {
        //My area
        if(e.Area.TeamId == player.TeamId)
        {
            InitiateMoveToFrisbee();
        }
        //other area
        else
        {

        }
    }

    private void Frisbee_OnWallBounce(object sender, System.EventArgs e)
    {
        if(frisbee.lastPlayer != null && frisbee.lastPlayer.TeamId != player.TeamId)
            InitiateMoveToFrisbee();
    }

    private void Frisbee_OnLaunched(object sender, Frisbee.FrisbeeEventArgs e)
    {
        //other player
        if (e.Player != player)
        {
            StartCoroutine(WaitForStateChange(BotState.MOVE_PREPARE_FOR_FRISBEE));
        }
        //this player
        else
        {
            StartCoroutine(WaitForStateChange(BotState.RECENTER));
        }
    }

    private void Frisbee_OnCaught(object sender, Frisbee.FrisbeeEventArgs e)
    {
        //other player
        if(e.Player != player)
        {
            State = BotState.RECENTER;
        }
        //this player
        else
        {
            State = BotState.LAUNCHING;
            StartCoroutine(WaitForLaunch());
        }
    }

    IEnumerator WaitForLaunch()
    {
        var time = Random.Range(0.2f, 1.5f);
        yield return new WaitForSeconds(time);
        Shoot();
    }

    IEnumerator WaitForStateChange(BotState state)
    {
        var time = Random.Range(ReactionTime.x, ReactionTime.y);
        yield return new WaitForSeconds(time);
        State = state;
    }

    #endregion

    // Update is called once per frame
    void Update () {

        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector3.zero;

        switch (State)
        {
            case BotState.MOVE_FOR_FRISBEE:
            case BotState.WAITING_FOR_OPPONENT:
                FollowFrisbee();
                break;
            case BotState.MOVE_PREPARE_FOR_FRISBEE:
                FollowFrisbee();
                break;
            case BotState.RECENTER:
                Recenter();
                break;
        }
    }

    #region MOVING

    Vector2 targetPosition;

    void InitiateMoveToFrisbee()
    {
        State = BotState.MOVE_FOR_FRISBEE;

        frisbee = GetFrisbee();
        targetPosition = frisbee.transform.position + (frisbee.direction * 5.0f);
    }

    void Move()
    {
        var moveVector = targetPosition - new Vector2(transform.position.x, transform.position.y);
        player.OnMove(moveVector);
        //Debug.Log(moveVector);
    }

    void FollowFrisbee()
    {
        var deltaY = frisbee.transform.position.y - transform.position.y;
        if (Mathf.Abs(deltaY) > 0.6f)
        {
            var moveVector = new Vector2(0, deltaY).normalized;
            player.OnMove(moveVector);
        }
    }

    void Recenter()
    {
        var moveVector = myArea.transform.position - transform.position;
        if (moveVector.magnitude < 1)
            State = BotState.WAITING_IDLE;
        moveVector = moveVector.normalized;
        player.OnMove(moveVector);
    }

    #endregion  

    #region SHOOTING

    void Shoot()
    {
        //bottom of the fan
        var shootType = Random.Range(0, 100);
        int i = 0; 

        var weightSum = ShootTypesList.Sum(x=>x.Weight);
        float weight = Random.Range(0,weightSum);
        float currentWeightSum = 0;

        while (i < ShootTypesList.Count)
        {
            var currentChance = ShootTypesList.ElementAt(i);
            currentWeightSum += currentChance.Weight;
            if (currentWeightSum > weight )
            {
                Shoot(currentChance);
                return;
            }
            i++;
        }

    }

    void Shoot(ShootTypeChance shoot)
    {
        RandomCurve(shoot.CurveChance);
        RandomPowerActivation(shoot.PowerChance);
        switch (shoot.ShootType)
        {
            case ShootTypeChance.Type.RANDOM:
                ShootRandom();
                break;
            case ShootTypeChance.Type.REBOUND:
                ShootRebound();
                break;
            case ShootTypeChance.Type.TOGOAL:
                ShootTowardsGoal();
                break;
        }
    }

    void ShootTowardsGoal()
    {
        //select a goal
        int goalIndex = Random.Range(0, opponentGoals.Count);
        var goal = opponentGoals.ElementAt(goalIndex);

        var toGoalVector = goal.transform.position - transform.position;
        toGoalVector = toGoalVector.normalized;

        //Debug.Log("SHOOTTOGOAL " + goal.gameObject.name);

        RandomCurve(15);
        player.OnLaunch(toGoalVector.x, toGoalVector.y);
    }

    void ShootRebound()
    {
        //bottom of the fan
        var angleBot = Random.Range(-70, -40);
        var angleTop = Random.Range(40, 70);

        var randomPercent = Random.Range(0, 100);

        var angle = randomPercent > 50 ? angleBot : angleTop;
        Vector2 v = Rotate(shootDirection, angle);
        v = v.normalized;

        //Debug.Log("SHOOTREBOUND " + angle + "  " + v);

        player.OnLaunch(v.x, v.y);
    }


    void ShootRandom()
    {
        //bottom of the fan
        var angle = Random.Range(-80, -80);

        Vector2 v = Rotate(shootDirection, angle);
        v = v.normalized;

        //Debug.Log("SHOOTRANDOM " + angle + "  " + v);

        RandomCurve(32);

        player.OnLaunch(v.x, v.y);
    }

    void RandomCurve(float curveChance)
    {
        var randomPercent = Random.Range(0, 100);
        if (randomPercent <= curveChance )
        {
            player.OnToggleCurve();
        }
    }

    void RandomPowerActivation(float powerChance)
    {
        var randomPercent = Random.Range(0, 100);
        if (randomPercent <= powerChance)
        {
            player.OnPowerActivate();
        }
    }

    #endregion


    Frisbee GetFrisbee()
    {
        var fFrisbee = frisbee;

        //chose a frisbee
        var frisbees = FindObjectsOfType<Frisbee>().Where(x=>x.gameObject.activeSelf).ToList();
        fFrisbee = frisbees.First(x => x.GetComponent<FakeFrisbee>() == null);

        if (frisbees.Count > 1)
        {
            int percent = Random.Range(0, 100);

            //% of chances to go to the wrong frisbee
            if (percent <= 66)
            {
                fFrisbee = frisbees.Where(x => x.GetComponent<FakeFrisbee>() != null).First();
            }
        }

        return fFrisbee;
    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    [System.Serializable]
    public class ShootTypeChance
    {
        public enum Type { TOGOAL, REBOUND, RANDOM }
        public Type ShootType;
        public float Weight;
        public float CurveChance;
        public float PowerChance;
    }
}
