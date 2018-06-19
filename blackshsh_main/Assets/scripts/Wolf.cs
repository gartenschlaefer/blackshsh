using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour {

  public Transform black_sheep;
  public Transform white_sheep;
  public Transform shepherd;

  private Vector3 wolf_start_pos;

  NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = gameObject.GetComponent<NavMeshAgent>();
    agent.SetDestination(black_sheep.position);
    wolf_start_pos = gameObject.GetComponent<Transform>().position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  // Collision
  void OnCollisionEnter(Collision collision){
    // TODO: Add rewards
    if (collision.gameObject.name == "black_sheep"){
      Debug.Log("Wolf eats black sheep");
      gameObject.transform.position = wolf_start_pos;
    }
    else if (collision.gameObject.name == "white_sheep"){
      Debug.Log("Wolf eats white sheep");
      gameObject.transform.position = wolf_start_pos;
    }
    else if (collision.gameObject.name == "shepherd"){
      Debug.Log("Wolf eats shepherd");
      gameObject.transform.position = wolf_start_pos;
    }
  }
}
