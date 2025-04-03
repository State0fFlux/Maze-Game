    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class OverheadLight : MonoBehaviour {
   
        public GameObject parent;
        private Vector3 initialPosition;

    void Start()
    {
        initialPosition = (transform.position - parent.transform.position);
    }

    void FixedUpdate () {
            transform.position = parent.transform.position + initialPosition;
        }
    }