using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputManager : MonoBehaviour {

    public GameObject AndroidInputObject;
    public Joystick joystick;
    public Joystick LaunchPad;

    public Player Player;
    public event EventHandler<EventArgs> OnPlayerSet;


	delegate void ActionDelegate();

	List<ActionDelegate> m_currentActions = new List<ActionDelegate> ();

	// Use this for initialization
	void Start () {
	}

	
	// Update is called once per frame
	void Update () {
		if (Player != null)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            Vector3 moveVector = new Vector3(x, 0, z);
            Player.OnMove(new Vector2(x,z));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Player.OnLaunch(x, z);
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
            Player.OnMove(moveVector);
        }


        if (launchVector.magnitude > 0.8f)
        {
            Player.OnLaunch(launchVector.x, launchVector.y);
        }
    }

	public void SetPlayer(Player _player){
        Player = _player;
        OnPlayerSet?.Invoke(this,null);
    }

    public void OnPowerActivate()
    {
        Player.OnPowerActivate();
    }

    public void OnToggleCurve()
    {
        Player.OnToggleCurve();
    }
    
	#region PLAYER_ACTION

	#endregion
}
