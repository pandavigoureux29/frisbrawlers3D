using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkConnectUI : MonoBehaviour {

	public GameNetworkManager manager;


	void Awake()
	{
		manager = GetComponent<GameNetworkManager>();
	}

	public void OnStartHostClicked(){
		manager.StartHost();
	}

	public void OnStartClientClicked(){
		manager.StartClient();
	}

    public void OnStartOfflineMode()
    {
        manager.OfflineMode = true;
        manager.StartHost();
    }

}
