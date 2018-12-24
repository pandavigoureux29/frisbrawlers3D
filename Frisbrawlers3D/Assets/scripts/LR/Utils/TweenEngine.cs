using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenEngine : MonoBehaviour {

	private static TweenEngine _instance;

	private List<Tween> m_tweens;

	public static TweenEngine instance {
		get{
			if( _instance == null ){
				GameObject newGO = new GameObject("TweenEngine");
				_instance = newGO.AddComponent<TweenEngine>();
			}
			return _instance;
		}
	}

	void Awake(){
		if (_instance == null)
			_instance = this;
		if (m_tweens == null)
			m_tweens = new List<Tween> ();
	}

	void Update(){
		//loop and update tweens
		for (int i= m_tweens.Count -1 ; i > -1; i --) {
			if( m_tweens[i].UpdateTween(Time.deltaTime) == true ){
				m_tweens.RemoveAt(i);
			}
		}
	}

	#region TWEENTRANSFORM

	/** Add a tween on position and start it. Time is in seconds */
	public TweenTransform PositionTo(Transform _target, Vector3 _destination, float _time ){
		TweenTransform tween = new TweenTransform (TweenTransform.Type.POSITION, _time, _target, _destination);
		m_tweens.Add (tween);
		return tween;
	}
	
	/** Add a tween on position and start it. Time is in seconds  
	* Callback receive an object as parameter
	* Callback object by default is the transform _target. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenTransform PositionTo(Transform _target, Vector3 _destination, float _time, string _callbackName ){
		TweenTransform tween = PositionTo (_target, _destination, _time);
		tween.CallbackName = _callbackName;
		return tween;
	}

	/** Add a tween on position and start it. Time is in seconds  
	* Callback receive an object as parameter (the target GameObject)
	* Callback object by default is the transform _target. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenTransform PositionTo(Transform _target, Vector3 _destination, float _time, bool _yoyo,int _repeat, string _callbackName ){
		TweenTransform tween = new TweenTransform (TweenTransform.Type.POSITION, _time, _target, _destination);
		tween.Yoyo = _yoyo;
		tween.Repeat = _repeat;
		tween.CallbackName = _callbackName;
		m_tweens.Add (tween);
		return tween;
	}

	//=============== SCALE ===================

	/** Add a tween on scale and start it. Time is in seconds  */
	public TweenTransform ScaleTo(Transform _target, Vector3 _destination, float _time ){
		TweenTransform tween = new TweenTransform (TweenTransform.Type.SCALE, _time, _target, _destination);
		m_tweens.Add (tween);
		return tween;
	}
	
	/** Add a tween on scale and start it. Time is in seconds  
	* Callback receive an object as parameter
	* Callback object by default is the transform _target. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenTransform ScaleTo(Transform _target, Vector3 _destination, float _time, string _callbackName ){
		TweenTransform tween = ScaleTo (_target, _destination, _time);
		tween.CallbackName = _callbackName;
		return tween;
	}
	
	/** Add a tween on scale and start it. Time is in seconds 
	* Callback receive an object as parameter
	* Callback object by default is the transform _target. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenTransform ScaleTo(Transform _target, Vector3 _destination, float _time, bool _yoyo,int _repeat, string _callbackName ){
		TweenTransform tween = new TweenTransform (TweenTransform.Type.SCALE, _time, _target, _destination);
		tween.Yoyo = _yoyo;
		tween.Repeat = _repeat;
		tween.CallbackName = _callbackName;
		m_tweens.Add (tween);
		return tween;
	}

	/** Add a tween on rotation around Z and start it. Time is in seconds 
	* Callback receive an object as parameter
	* Callback object by default is the transform _target. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenTransform RotateAroundZTo(Transform _target, float _destination, float _time ){
		Vector3 dest = _target.localEulerAngles;
		dest.z = _destination;
		TweenTransform tween = new TweenTransform (TweenTransform.Type.ROTATION, _time, _target, dest);
		m_tweens.Add (tween);
		return tween;
	}

	/** Add a tween on rotation around Z and start it. Time is in seconds 
	* Callback receive an object as parameter
	* Callback object by default is the transform _target. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenTransform RotateAroundZTo(Transform _target, float _destination, float _time, bool _yoyo,int _repeat, string _callbackName ){
		Vector3 dest = _target.localEulerAngles;
		dest.z = _destination;
		TweenTransform tween = new TweenTransform (TweenTransform.Type.ROTATION, _time, _target, dest);
		tween.Yoyo = _yoyo;
		tween.Repeat = _repeat;
		tween.CallbackName = _callbackName;
		m_tweens.Add (tween);
		return tween;
	}

	#endregion

	#region TWEENCOLOR
	/** Add a tween on alpha and start it. Time is in seconds */
	public TweenColor AlphaTo(SpriteRenderer _target, float _destination, float _time ){
		TweenColor tween = new TweenColor (_time, _target, _destination);
		m_tweens.Add (tween);
		return tween;
	}

	/** Add a tween on alpha and start it. Time is in seconds  
	* Callback receive an object as parameter 
	* Callback object by default is the GameObject holding the SpriteRenderer. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenColor AlphaTo(SpriteRenderer _target, float _destination, float _time, string _callbackName ){
		TweenColor tween = AlphaTo (_target, _destination, _time);
		tween.CallbackName = _callbackName;
		return tween;
	}

	//ALL
	/** Add a tween on alpha and start it. Time is in seconds 
	* Callback receive an object as parameter 
	* Callback object by default is the GameObject holding the SpriteRenderer. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenColor AlphaTo(SpriteRenderer _target, float _destination, float _time, bool _yoyo, int _repeat,string _callbackName ){
		TweenColor tween = AlphaTo (_target, _destination, _time);
		tween.Yoyo = _yoyo;
		tween.Repeat = _repeat;
		tween.CallbackName = _callbackName;
		return tween;
	}

	/** Add a tween on alpha and start it. Time is in seconds */
	public TweenColor ColorTo(SpriteRenderer _target, Color _destination, float _time ){
		TweenColor tween = new TweenColor (_time, _target, _destination);
		m_tweens.Add (tween);
		return tween;
	}
	
	/** Add a tween on color and start it. Time is in seconds 
	  *Callback receive an object as parameter 
	* Callback object by default is the GameObject holding the SpriteRenderer. Set custom with TweenEngine.Tween.CallbackObject
	 */
	public TweenColor ColorTo(SpriteRenderer _target, Color _destination, float _time, string _callbackName ){
		TweenColor tween = ColorTo (_target, _destination, _time);
		tween.CallbackName = _callbackName;
		return tween;
	}

	//ALL
	/** Add a tween on color and start it. Time is in seconds 
	 * Callback receive an object as parameter*/
	public TweenColor ColorTo(SpriteRenderer _target, Color _destination, float _time,bool _yoyo, int _repeat, string _callbackName ){
		TweenColor tween = ColorTo (_target, _destination, _time);
		tween.Yoyo = _yoyo;
		tween.Repeat = _repeat;
		tween.CallbackName = _callbackName;
		return tween;
	}

	#endregion

	#region INNER_CLASS_TWEENs

	public class Tween{
		protected float m_time;
		protected float m_targetTime;
		protected bool m_finished = false;
		protected bool m_stopped = false;

		protected bool m_yoyo = false;
		protected bool m_forward = true;
		protected int m_repeat = 0;
		protected int m_count = 0;

		protected string m_callbackName;
		protected GameObject m_callbackObject;

		public Tween(float _targetTime){
			m_targetTime = _targetTime;
		}

		/** Update the tween and return true if ended */
		virtual public bool UpdateTween( float _deltaTime ){ return false;}

		protected void SendCallback(GameObject _target, object _value){
			//if the m_callbackObject prop is set, it overrides the target
			if (m_callbackObject != null) {
				_target = m_callbackObject;
			}
			//send callback
			if(_target != null && m_callbackName != null && m_callbackName != "" )
				_target.SendMessage(m_callbackName,_value, SendMessageOptions.DontRequireReceiver);
		}

		protected void IncrementCount(){
			if (m_yoyo) {
				if( m_forward == false )
					m_count ++;
				m_finished = !m_forward && ( m_repeat < m_count );
			} else {
				m_count ++;
				m_finished = m_repeat < m_count;
			}
		}
		
		public void Stop( bool _sendCallback ){
			m_stopped = true;
		}

		public bool IsFinished {
			get {
				return m_finished;
			}
		}

		public string CallbackName {
			get {
				return m_callbackName;
			}
			set {
				m_callbackName = value;
			}
		}

		/** Use this accessor to change the target of the callback */
		public GameObject CallbackObject{
			get{
				return m_callbackObject;
			}
			set{
				m_callbackObject = value;
			}			
		}

		public bool Yoyo {
			get {
				return m_yoyo;
			}
			set {
				m_yoyo = value;
			}
		}

		public int Repeat {
			get {
				return m_repeat;
			}
			set {
				m_repeat = value;
			}
		}

        public virtual object Target {
            get { return null; }
        }
	}

	public class TweenTransform : Tween{
		protected Transform m_target;
		protected Vector3 m_destination;
		protected Vector3 m_currentDestination;

		protected Vector3 m_srcVector;

		public enum Type {POSITION, ROTATION, SCALE};
		protected TweenTransform.Type m_type = TweenTransform.Type.POSITION;

		public TweenTransform(TweenTransform.Type _type,float _time, Transform _target, Vector3 _destination) : base(_time){
			m_target = _target;
			m_destination = _destination;
			m_currentDestination = m_destination;
			m_type = _type;
			switch(m_type){
				case Type.POSITION : m_srcVector = m_target.localPosition;break;
				case Type.ROTATION : m_srcVector = m_target.localEulerAngles;break;
				case Type.SCALE : m_srcVector = m_target.localScale;break;
			}

		}
		
		override public bool UpdateTween( float _deltaTime ){
			if (m_stopped) {
				SendCallback( m_target.gameObject, (object) m_target.gameObject);
				return true;
			}

			m_time += _deltaTime;
			//if time's up
			if( m_time >= m_targetTime){

				IncrementCount();

				switch(m_type){
					case Type.POSITION : m_target.localPosition = m_currentDestination;break;
					case Type.ROTATION : m_target.localEulerAngles = m_currentDestination ;break;
					case Type.SCALE : m_target.localScale = m_currentDestination ;break;
				}
									
				//if really finished
				if( m_finished ){
					SendCallback( m_target.gameObject, (object) m_target.gameObject);
				}else{
					m_time = 0;
					//change destination in yoyo
					if( m_yoyo ){
						if( m_forward )
							m_currentDestination = m_srcVector;
						else
							m_currentDestination = m_destination;
						m_forward = !m_forward;
					}
				}
				return m_finished;
			}
			//compute progression
			float progression = m_time / m_targetTime;
			Vector3 tempVector;
			if (m_forward)
				tempVector = m_srcVector + ((m_destination - m_srcVector) * progression);
			else
				tempVector = m_destination + ((m_srcVector - m_destination) * progression);
			switch(m_type){
				case Type.POSITION : m_target.localPosition = tempVector;break;
				case Type.ROTATION : m_target.localEulerAngles = tempVector ;break;
				case Type.SCALE : m_target.localScale = tempVector ;break;
			}

			return false;
		}

        public override object Target
        {
            get { return m_target; }
        }
    }

	public class TweenColor : Tween{
		protected SpriteRenderer m_target;
		protected Color m_destination;
		protected Color m_currentDestination;
		
		protected Color m_srcColor;

		public TweenColor(float _time, SpriteRenderer _target, Color _destination) : base(_time){
			m_target = _target;
			m_destination = _destination;
			m_currentDestination = m_destination;
			m_srcColor = _target.color;	
		}

		public TweenColor(float _time, SpriteRenderer _target, float _alphaDest) : base(_time){
			m_target = _target;
			m_destination = m_target.color;
			m_destination.a = _alphaDest;
			m_currentDestination = m_destination;
			m_srcColor = _target.color;	
		}
		
		override public bool UpdateTween( float _deltaTime ){
            if(m_target == null)
            {
                m_finished = true;
                return true;
            }

			if (m_stopped) {
				SendCallback( m_target.gameObject, (object) m_target);
				return true;
			}
			m_time += _deltaTime;
			//if time's up
			if( m_time >= m_targetTime){
				IncrementCount();
				m_target.color = m_currentDestination; 
				//if really finished
				if( m_finished ){
					SendCallback( m_target.gameObject, (object) m_target);
				}else{
					m_time = 0;
					//change destination in yoyo
					if( m_yoyo ){
						if( m_forward )
							m_currentDestination = m_srcColor;
						else
							m_currentDestination = m_destination;
						m_forward = !m_forward;
					}
				}
				return m_finished;
			}
			//compute progression
			float progression = m_time / m_targetTime;
			if( m_forward )
				m_target.color = m_srcColor + ( (m_destination - m_srcColor) * progression );
			else
				m_target.color = m_destination + ( (m_srcColor - m_destination) * progression );
			
			return false;
		}

        public override object Target
        {
            get { return m_target; }
        }
    }

	#endregion
}
