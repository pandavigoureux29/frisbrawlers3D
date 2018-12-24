using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour {

    [SerializeField] GameMatchManager m_matchManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Frisbee"))
        {
            m_matchManager.Goal(false, 0);
        }
    }
}
