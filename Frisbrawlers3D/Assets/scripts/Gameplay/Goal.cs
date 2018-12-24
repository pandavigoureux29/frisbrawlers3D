using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	[SerializeField] GameMatchManager m_matchManager;
	[SerializeField] public bool teamA = true;

    public int Points = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D _collider){
		if(_collider.gameObject.layer == LayerMask.NameToLayer ("Frisbee")) {
            var fake = _collider.gameObject.GetComponent<FakeFrisbee>();
            if(fake == null)
            {
                m_matchManager.Goal(teamA, Points);
            }
		}
	}
}
