using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameNetworkManager : NetworkManager {

	GameMatchManager m_matchManager;
	int NbPlayerConnected = 0;
	bool GameLaunched = false;

    public bool OfflineMode = false;
    public GameObject AIPrefab;

	public GameMatchManager MatchManager {
		get {
			if (m_matchManager == null)
				m_matchManager = FindObjectOfType<GameMatchManager> ();
			return m_matchManager;
		}
	}

	void Start(){
		m_teams.Add (new Team (){ Id = 0 });
		m_teams.Add (new Team (){ Id = 1 });
	}

	void Update(){
		//launch game when all players are connected and scene is loaded
		if (GameLaunched == false && /*NbPlayerConnected == MaxTeamPlayerCount * 2 &&*/  MatchManager != null) {
			GameLaunched = true;
            if (OfflineMode)
                CreateAI();
			foreach (var team in m_teams) {
				foreach (var player in team.Players) {
					MatchManager.SpawnPlayer (player, team.Id);
                    if (team.Id > 0)
                        player.Reverse();
                }
			}
            MatchManager.KickOff();
		}
	}

    public void CreateAI()
    {
        var go = Instantiate(AIPrefab) as GameObject;

        //Get Player component and affect a team
        var player = go.GetComponent<Player>();
        int teamId = AddPlayerToTeam(player);
        //NetworkServer.AddPlayerForConnection(conn, go, playerControllerId);
        NbPlayerConnected++;
    }
	
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId, NetworkReader extraMsg)
	{
		GameObject go = null;
		if (extraMsg != null) {
			var msg = extraMsg.ReadMessage<StringMessage> ();
			string prefabPath = msg.value;
			//load and registrer new player prefab
			var prefab = Resources.Load (prefabPath) as GameObject;
			ClientScene.RegisterPrefab (prefab);
			go = (GameObject)GameObject.Instantiate (prefab);
		} else {
			//instantiate default player
			go = (GameObject)GameObject.Instantiate (playerPrefab);
		}

		//Get Player component and affect a team
		var player = go.GetComponent<Player> ();
		int teamId = AddPlayerToTeam (player); 

		if( GameLaunched )
			MatchManager.SpawnPlayer(player, teamId);

		NetworkServer.AddPlayerForConnection (conn, go, playerControllerId);
		NbPlayerConnected++;
	}


	public override void OnClientConnect(NetworkConnection conn)
	{		
		if (!clientLoadedScene) {
			RegisterPrefabAndAddPlayer (conn);
		}
	}

	public override void OnClientSceneChanged (NetworkConnection conn)
	{
		ClientScene.Ready (conn);

		//check if we need to add a player
		bool addPlayer = (ClientScene.localPlayers.Count == 0);
		bool foundPlayer = false;
		foreach (var player in ClientScene.localPlayers) {
			if (player.gameObject != null) {
				foundPlayer = true;
			}
		}

		if (!foundPlayer) {
			addPlayer = true;
		}
		if (addPlayer) {
			RegisterPrefabAndAddPlayer (conn);
		}
	}

	void RegisterPrefabAndAddPlayer(NetworkConnection conn){
		string playerPrefabName = "prefabs/players/blue_shrimp";
		var prefab = Resources.Load (playerPrefabName) as GameObject;
		ClientScene.RegisterPrefab (prefab);

		StringMessage message = new StringMessage (playerPrefabName);

		//call add player with the message 
		ClientScene.AddPlayer(conn, 1,message);
	}

	#region TEAMS

	List<Team> m_teams = new List<Team> ();
	public int MaxTeamPlayerCount = 1;

	public int AddPlayerToTeam(Player player){
		var team = GetTeamWithLessPlayers ();
		team.Players.Add (player);
		player.SetTeam( team.Id );
		return team.Id;
	}

	Team GetTeamWithLessPlayers(){
		return m_teams [0].NbPlayers <= m_teams [1].NbPlayers ? m_teams [0] : m_teams [1];
	}

	class Team {
		public int Id;
		public List<Player> Players = new List<Player>();

		public int NbPlayers { get { return Players.Count; } }
	}

	#endregion
}
