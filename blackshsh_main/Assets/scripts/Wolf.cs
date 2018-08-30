using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wolf : MonoBehaviour {

  public Transform black_sheep;
  public Transform white_sheep;
  public Transform shepherd;

  public float follow_shepherd_dist;
  public float follow_white_sheep_dist;

  [HideInInspector]
  public bool follow_shepherd;
  [HideInInspector]
  public bool follow_white_sheep;

  NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = gameObject.GetComponent<NavMeshAgent>();
    agent.SetDestination(black_sheep.position);
	}
	
	// Update is called once per frame
	void Update () {
    // calculate distances
    Vector3 wolf_pos = gameObject.GetComponent<Transform>().position;
    float shepherd_dist = Vector3.Distance(wolf_pos, shepherd.position);
		float white_sheep_dist = Vector3.Distance(wolf_pos, white_sheep.position);
    //Debug.Log("distance: " + shepherd_dist + white_sheep_dist);
    // follow closest object
    if (white_sheep_dist < follow_white_sheep_dist){
      agent.SetDestination(white_sheep.position);
      follow_shepherd = false;
      follow_white_sheep = true;
    }
    else if (shepherd_dist < follow_shepherd_dist){
      agent.SetDestination(shepherd.position);
      follow_shepherd = true;
      follow_white_sheep = false;
    }
    else{
      // check if black_sheep is there
      if (black_sheep == null){
        agent.SetDestination(shepherd.position);
        follow_shepherd = true;
        follow_white_sheep = false;
      }
      else{
        agent.SetDestination(black_sheep.position);
        follow_shepherd = false;
        follow_white_sheep = false;
      }
    }
	}

  // Collision
  void OnCollisionEnter(Collision collision){
    if (collision.gameObject.name == "black_sheep"){
      Debug.Log("Wolf eats black sheep");
      shepherd.GetComponent<ShepherdAgent>().BlackSheepDead();
    }
    else if (collision.gameObject.name == "white_sheep"){
      Debug.Log("Wolf eats white sheep");
      shepherd.GetComponent<ShepherdAgent>().WhiteSheepSacrifice();
    }
    else if (collision.gameObject.name == "shepherd"){
      Debug.Log("Wolf eats shepherd");
      shepherd.GetComponent<ShepherdAgent>().MySelfDead();
    }
  }
}
