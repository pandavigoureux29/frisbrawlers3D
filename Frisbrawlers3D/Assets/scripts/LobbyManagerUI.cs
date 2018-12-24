using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyManagerUI : MonoBehaviour {
	
	public NetworkManager manager;

	public GameObject LobbyObject;

	public GameObject JoinRoomButton;

	public string DefaultRoomName = "default";

    public Text IPText;

	// Use this for initialization
	void Start () {
		LobbyObject.SetActive (false);
		JoinRoomButton.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
        IPText.text = GetIP();
    }

    string GetIP()
    {
        var strHostName = "";
        strHostName = System.Net.Dns.GetHostName();

        var ipEntry = System.Net.Dns.GetHostEntry(strHostName);

        var addr = ipEntry.AddressList;

        return addr[addr.Length - 1].ToString();
    }

	public void OnEnableLobby(){
		manager.StartMatchMaker();
		manager.matchName = DefaultRoomName;
		manager.SetMatchHost("mm.unet.unity3d.com", 443, true); //internet

		LobbyObject.SetActive (true);
	}

	public void OnDisableLobby(){
		manager.StopMatchMaker();
		LobbyObject.SetActive (false);
	}

	public void OnCreateRoomClicked(){
		manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
	}

	public void OnFindRoomsClicked(){
		manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
		JoinRoomButton.SetActive (true);
	}

	public void OnJoinRoomClicked(){

        Debug.Log(manager.matches.Count + "matches found");
        for (int i = 0; i < manager.matches.Count; i++)
		{
			var matchn = manager.matches[i];
			/*if (GUI.Button(new Rect(100, 100 + i *30, 200, 20), "Join Match:" + matchn.name))
			{
				manager.matchName = matchn.name;
				manager.matchMaker.JoinMatch(matchn.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
			}*/
		}
        //Debug.Log(manager.matches);
		var match = manager.matches.FirstOrDefault ( x => x.name == DefaultRoomName);
		manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
	}

}
