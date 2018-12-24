using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameMatchManager : MonoBehaviour {

	[SerializeField] Transform m_ballSpawnPoint;
	[SerializeField] GameObject m_ballPrefab;
	Transform m_frisbee;

	[SerializeField] List<Transform> m_teamASpawnPoints;
	[SerializeField] List<Transform> m_teamBSpawnPoints;

	List<Team> m_teams = new List<Team>();

    [SerializeField] TextMesh m_scoreA;
    [SerializeField] TextMesh m_scoreB;

    public event EventHandler<EventArgs> OnGoal;

    private void Awake()
    {
        //spawn ball on the server
        var go = Instantiate(m_ballPrefab);
        m_frisbee = go.transform;
        ResetFrisbee();
        CreateTeams();
    }

    // Use this for initialization
    void Start () {
	}

    public void KickOff()
    {
        var right = m_teams[0].Score > m_teams[1].Score ? true : false;
        if (m_teams[1].Players.Count == 0)
            right = false;
        m_frisbee.GetComponent<Frisbee>().KickOff(right);
    }


	#region TEAMS

	void CreateTeams(){
		m_teams.Add (new Team (0, m_teamASpawnPoints));
		m_teams.Add (new Team (1, m_teamBSpawnPoints));
	}

	/// <summary>
	/// Set a player's team ans sets its position
	/// </summary>
	public void SpawnPlayer(Player _player, int _teamId){
		Team team= m_teams.FirstOrDefault(x=>x.Id == _teamId);
		team.SpawnPlayer (_player);
	}


	#endregion

	public void Goal(bool teamA, int points){
		var team = teamA ? m_teams [1] : m_teams [0];
        team.Score+= points;

        var scoreTxt = teamA ? m_scoreB : m_scoreA;
        scoreTxt.text = team.Score.ToString();

        OnGoal?.Invoke(this, null);

        ResetGame ();
	}

	public void ResetGame(){
		ResetFrisbee ();
		foreach (var team in m_teams) {
			team.ResetSpawnPoints ();
			for(int i=0; i < team.Players.Count; i++){
				var player = team.Players [i];
				ResetBody (player.GetComponent<Rigidbody>());
				SpawnPlayer (player, player.TeamId);
			}
        }
        StartCoroutine(WaitForKickOff());
	}

	public void ResetFrisbee(){
        m_frisbee.GetComponent<Frisbee>().ResetFrisbee();
		m_frisbee.position = m_ballSpawnPoint.transform.position;
		ResetBody (m_frisbee.GetComponent<Rigidbody> ());
	}

	public void ResetBody(Rigidbody _rigidbody){
		_rigidbody.velocity = new Vector3(0f,0f); 
		_rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector2.zero;
	}

	public class Team {
		public int Id { get; set; }

		public List<Player> Players = new List<Player>();

		public List<SpawnPoint> SpawnPoints = new List<SpawnPoint> ();

		public int Score = 0;

		public int NbPlayers{
			get{ return Players.Count; }
		}

		public Team(){}
		public Team(int _id, List<Transform> _spawnPointsList){
			Id = _id;
			foreach(var spPt in _spawnPointsList){
				SpawnPoints.Add(new SpawnPoint(spPt));
			}
		}

		public void SpawnPlayer(Player _player){
			//get random spawnpoint within available ones
			var availableSpPt = SpawnPoints.Where (x => x.Used == false);
			int i = UnityEngine.Random.Range(0,SpawnPoints.Count - 1);
			var sp = SpawnPoints [i];

			//spawn player and set as used
			_player.transform.position = sp.Transform.position;
			//_player.GetComponent<Rigidbody>().MoveRotation (sp.Transform.rotation.eulerAngles.z);
			sp.Used = true;

			if(!Players.Contains(_player))
				Players.Add (_player);
		}

		public void ResetSpawnPoints(){
			foreach (var spPt in SpawnPoints) {
				spPt.Used = false;
			}
		}
	}

    IEnumerator WaitForKickOff()
    {
        yield return new WaitForSeconds(1.5f);
        KickOff();
    }

    public class SpawnPoint{
		public Transform Transform {get;set;}
		public bool Used { get; set; }

		public SpawnPoint(){ Used = false;}
		public SpawnPoint(Transform _t){
			Transform = _t;
			Used = false;
		}
	}
}
