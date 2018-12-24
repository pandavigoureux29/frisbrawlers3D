using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputManager : MonoBehaviour {

    public GameObject AndroidInputObject;
    public Joystick joystick;
    public Joystick LaunchPad;

	Player m_player;
    public Player Player { get { return m_player; } }
    public event EventHandler<EventArgs> OnPlayerSet;


	delegate void ActionDelegate();

	List<ActionDelegate> m_currentActions = new List<ActionDelegate> ();

	// Use this for initialization
	void Start () {
	}

	
	// Update is called once per frame
	void Update () {
		if (m_player != null)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            Vector3 moveVector = new Vector3(x, 0, z);
            m_player.OnMove(new Vector2(x,z));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_player.OnLaunch(x, z);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                OnPowerActivate();
            }

            CheckAndroidInput();

            /*foreach (var action in m_currentActions) {
				action ();
			}*/
        }
	
	}

    public void CheckAndroidInput()
    {
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.up * joystick.Vertical);
        Vector3 launchVector = (Vector3.right * LaunchPad.Horizontal + Vector3.up * LaunchPad.Vertical);


        if (moveVector != Vector3.zero)
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
            //transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
            m_player.OnMove(moveVector);
        }


        if (launchVector.magnitude > 0.8f)
        {
            m_player.OnLaunch(launchVector.x, launchVector.y);
        }
    }

	public void SetPlayer(Player _player){
		m_player = _player;
        OnPlayerSet?.Invoke(this,null);

    }

    public void OnPowerActivate()
    {
        m_player.OnPowerActivate();
    }

    public void OnToggleCurve()
    {
        m_player.OnToggleCurve();
    }
    
	#region PLAYER_ACTION

	#endregion
}
