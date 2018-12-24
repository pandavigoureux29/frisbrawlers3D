using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public partial class Utils  {

	#region LOCALPOSITION
	public static Vector3 SetLocalPositionX(Transform _t, float _newValue){
		Vector3 tmpVector = _t.localPosition;
		tmpVector.x = _newValue;
		_t.localPosition = tmpVector;
		return _t.localPosition;
	}

	public static Vector3 SetLocalPositionY(Transform _t, float _newValue){
		Vector3 tmpVector = _t.localPosition;
		tmpVector.y = _newValue;
		_t.localPosition = tmpVector;
		return _t.localPosition;
	}

	public static Vector3 SetLocalPositionZ(Transform _t, float _newValue){
		Vector3 tmpVector = _t.localPosition;
		tmpVector.z = _newValue;
		_t.localPosition = tmpVector;
		return _t.localPosition;
    }

    public static Vector3 SetLocalPositionXY(Transform _t, float _x, float _y)
    {
        Vector3 tmpVector = _t.localPosition;
        tmpVector.x = _x;
        tmpVector.y = _y;
        _t.localPosition = tmpVector;
        return _t.localPosition;
    }

    /// <summary>
    /// Sets the local position without the Z axis
    /// </summary>
    public static Vector3 Set2DLocalPosition(Transform _t, Vector3 position)
    {
        Vector3 tmpVector = _t.localPosition;
        tmpVector.x = position.x;
        tmpVector.y = position.y;
        _t.localPosition = tmpVector;
        return _t.localPosition;
    }
    #endregion

    #region POSITION
    public static Vector3 SetPositionX(Transform _t, float _newValue){
		Vector3 tmpVector = _t.position;
		tmpVector.x = _newValue;
		_t.position = tmpVector;
		return _t.position;
	}

	public static Vector3 SetPositionY(Transform _t, float _newValue){
		Vector3 tmpVector = _t.position;
		tmpVector.y = _newValue;
		_t.position = tmpVector;
		return _t.position;
	}

	public static Vector3 SetPositionZ(Transform _t, float _newValue){
		Vector3 tmpVector = _t.position;
		tmpVector.z = _newValue;
		_t.position = tmpVector;
		return _t.position;
	}

    public static Vector3 SetPositionXY(Transform _t, float _x, float _y)
    {
        Vector3 tmpVector = _t.position;
        tmpVector.x = _x;
        tmpVector.y = _y;
        _t.position = tmpVector;
        return _t.position;
    }

    /// <summary>
    /// Sets the position without the Z axis
    /// </summary>
    public static Vector3 Set2DPosition(Transform _t, Vector3 position)
    {
        Vector3 tmpVector = _t.position;
        tmpVector.x = position.x;
        tmpVector.y = position.y;
        _t.position = tmpVector;
        return _t.position;
    }
    #endregion

    #region LOCALSCALE
    public static Vector3 SetLocalScaleX(Transform _t, float _newValue){
		Vector3 tmpVector = _t.localScale;
		tmpVector.x = _newValue;
		_t.localScale = tmpVector;
		return _t.localScale;
	}

	public static Vector3 SetLocalScaleY(Transform _t, float _newValue){
		Vector3 tmpVector = _t.localScale;
		tmpVector.y = _newValue;
		_t.localScale = tmpVector;
		return _t.localScale;
	}
    public static Vector3 SetLocalScaleXY(Transform _t, float _newScaleX, float _newScaleY)
    {
        Vector3 tmpVector = _t.localScale;
        tmpVector.y = _newScaleY;
        tmpVector.x = _newScaleX;
        _t.localScale = tmpVector;
        return _t.localScale;
    }

    #endregion

    public static Vector2 GetVector2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 SetLocalAngleZ(Transform _t, float _newValue){
		Vector3 tmpVector = _t.localEulerAngles;
		tmpVector.z = _newValue;
		_t.localEulerAngles = tmpVector;
		return _t.localEulerAngles;
	}
    
	public static void SetAlpha(SpriteRenderer _spriteRenderer,float _alpha){
		Color color = _spriteRenderer.color;
		color.a = _alpha;
		_spriteRenderer.color = color;
	}

	public static void SetAlpha(TextMesh _textMesh,float _alpha){
		Color color = _textMesh.color;
		color.a = _alpha;
		_textMesh.color = color;
	}

    /// <summary>
    /// Returns true if the animation is running. 
    /// If _checkTransition, also returns false if the animator is still in ANY transition.
    /// </summary>
	public static bool IsAnimationStateRunning( Animator _animator, string _statename, bool _checkTransition = true, int _layerIndex = 0){
        bool result = _animator.GetCurrentAnimatorStateInfo(_layerIndex).IsName(_statename);
        if (_checkTransition)
            result = result && !_animator.IsInTransition(_layerIndex);
        return result;
	}
    
	public static void SetParentKeepTransform( Transform _child, Transform _parent){
		Vector3 pos = _child.localPosition;
		Vector3 scale = _child.localScale;
		Quaternion q = _child.localRotation;

		_child.SetParent (_parent);

		_child.localPosition = pos;
		_child.localScale = scale;
		_child.localRotation = q ;
	}

	public static bool IsValidString(string _str){
		return _str != null && _str != "";
	}

    /// <summary>
    /// Returns a SIGNED angle between the two vector ( counterclockwise )
    /// </summary>
    public static float AngleBetweenVectors(Vector3 v1, Vector3 v2)
    {
        return Mathf.DeltaAngle(Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg,
                                Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg);
    }    

    /// <summary>
    /// Copies the properties to a destination object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public static void CopyProperties(object source, object destination)
    {
        // If any this null throw an exception
        if (source == null || destination == null)
            throw new System.Exception("Source or/and Destination Objects are null");
        // Getting the Types of the objects
        Type typeDest = destination.GetType();
        Type typeSrc = source.GetType();

        // Iterate the Properties of the source instance and  
        // populate them from their desination counterparts  
        PropertyInfo[] srcProps = typeSrc.GetProperties();
        foreach (PropertyInfo srcProp in srcProps)
        {
            if (!srcProp.CanRead)
            {
                continue;
            }
            PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
            if (targetProperty == null)
            {
                continue;
            }
            if (!targetProperty.CanWrite)
            {
                continue;
            }
            if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
            {
                continue;
            }
            if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
            {
                continue;
            }
            if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
            {
                continue;
            }
            // Passed all tests, lets set the value
            targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
        }
    }

    /// <summary>
    /// Returns the number of elements inside an enum. 
    /// Usage : EnumCount( MyEnum.AnyValue )
    /// </summary>
    public static int EnumCount( Enum _enum ){        
        return Enum.GetNames(_enum.GetType()).Length;
    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    public static void ConvertToUIImage( GameObject _go )
    {
        var spritesRenderers = _go.GetComponentsInChildren<SpriteRenderer>();
        foreach(var sR in spritesRenderers)
        {
            var uiImage = sR.GetComponent<UnityEngine.UI.Image>();
            if( uiImage == null)
            {
                uiImage = sR.gameObject.AddComponent<UnityEngine.UI.Image>();
            }
            uiImage.sprite = sR.sprite;
            uiImage.color = sR.color;
            sR.enabled = false;
            uiImage.enabled = true;            
        }
    }
    
    public static Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    #region INPUT
    public static bool MouseJustPressed()
    {
    #if UNITY_ANDROID
        bool b = false;
        for (int i = 0; i < Input.touches.Length; i++)
        {
            b = Input.touches[i].phase == TouchPhase.Began;
            if (b)
                break;
        }
        return b;
    #else        
        return Input.GetMouseButtonDown(0);
    #endif
    }

    public static bool MouseJustReleased()
    {
    #if UNITY_ANDROID
        bool b = false;
        for (int i = 0; i < Input.touches.Length; i++)
        {
            b = Input.touches[i].phase == TouchPhase.Ended;
            if (b)
                break;
        }
        return b;
    #else        
        return Input.GetMouseButtonUp(0);
    #endif
    }
    #endregion
}
