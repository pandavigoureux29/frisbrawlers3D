using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerEngine : MonoBehaviour {

	private static TimerEngine _instance;
	
	private List<Timer> m_timers;

	private List<Timer> m_toRemove = new List<Timer>();
	
	public static TimerEngine instance {
		get{
			if( _instance == null ){
				GameObject newGO = new GameObject("TimerEngine");
				_instance = newGO.AddComponent<TimerEngine>();
			}
			return _instance;
		}
	}

	void Awake(){
		if (_instance == null)
			_instance = this;
		if (m_timers == null)
			m_timers = new List<Timer> ();
	}
	
	// Update is called once per frame
	void Update () {
		//loop and update timers
		for (int i= m_timers.Count -1 ; i > -1; i --) {
			//Remove finished timers
			if( m_timers[i].UpdateTimer(Time.deltaTime) == true ){
				m_timers.RemoveAt(i);
			}
		}

		//Remove timers that has been stopped manually
		if (m_toRemove.Count > 0) {
			for (int i= 0; i < m_toRemove.Count; i++) {
				//Remove finished timers
				int index = m_timers.IndexOf( m_toRemove[i] );
				if( index != -1 ){
					m_timers.RemoveAt(index);
				}
			}
			m_toRemove.Clear ();
		}
	}

	/// <summary>
	///  Add a timer and start it. Time is in seconds
	/// </summary>
	public Timer AddTimer(float _time, string _callbackBackName, GameObject _callbackObject){
		Timer timer = new Timer (_time, _callbackBackName,_callbackObject);
		m_timers.Add (timer);
		return timer;
	}

    /// <summary>
    /// Stops the first timer corresponding with the callback. You can also specify the target gameobject
    /// </summary>
    public void StopFirst(string _callbackName, GameObject _callbackObject = null)
    {
        for (int i = m_timers.Count - 1; i > -1; i--) {
            Timer timer = m_timers[i];
            if( timer.CallbackName == _callbackName && (_callbackObject!=null && timer.CallbackObject == _callbackObject))
            {
				m_toRemove.Add (timer);
                return;
            }
        }
    }

    /// <summary>
    /// Stops all the timers corresponding with the callback. You can also specify the target gameobject
    /// </summary>
    public void StopAll(string _callbackName, GameObject _callbackObject = null)
    {
        for (int i = m_timers.Count - 1; i > -1; i--)
        {
            Timer timer = m_timers[i];
            if (timer.CallbackName == _callbackName && (_callbackObject != null && timer.CallbackObject == _callbackObject))
            {
				m_toRemove.Add (timer);
            }
        }
    }

    public class Timer{
		protected float m_time;
		protected float m_targetTime;
		protected bool m_finished = false;
		
		protected string m_callbackName;
		protected GameObject m_callbackObject;
		
		public Timer(float _targetTime, string _callbackName, GameObject _callbackObject){
			m_targetTime = _targetTime;
			m_callbackName = _callbackName;
			m_callbackObject = _callbackObject;
		}

		protected void SendCallback(){
			//send callback
			if(m_callbackObject != null && m_callbackName != null && m_callbackName != "" )
				m_callbackObject.SendMessage(m_callbackName);
		}
		
		/** Update the timer and return true if ended */
		public bool UpdateTimer( float _deltaTime ){ 
			
			m_time += _deltaTime;
			//if time's up
			if( m_time >= m_targetTime){
				m_finished = true;
				SendCallback();
				return true;
			}
			return false;
		}

        public float Time {get { return m_time; } }
        public string CallbackName { get { return m_callbackName; } }
        public GameObject CallbackObject { get { return m_callbackObject; } }
        public bool Finished { get { return m_finished; } }
	}
}
