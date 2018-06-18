using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour {

  public Transform black_sheep;
  public Transform white_sheep;
  public Transform shepherd;
  NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = this.GetComponent<NavMeshAgent>();
    agent.SetDestination(black_sheep.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
