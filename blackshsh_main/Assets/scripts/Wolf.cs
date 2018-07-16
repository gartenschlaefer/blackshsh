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
    // calculate distances
    Vector3 wolf_pos = gameObject.GetComponent<Transform>().position;
    float shepherd_dist = Vector3.Distance(wolf_pos, shepherd.position);
		float white_sheep_dist = Vector3.Distance(wolf_pos, white_sheep.position);
    //Debug.Log("distance: " + shepherd_dist + white_sheep_dist);
    // follow closest object
    if (white_sheep_dist < follow_white_sheep_dist){
      agent.SetDestination(white_sheep.position);
      follow_shepherd = false;
    }
    else if (shepherd_dist < follow_shepherd_dist){
      agent.SetDestination(shepherd.position);
      follow_shepherd = true;
    }
    else{
      agent.SetDestination(black_sheep.position);
      follow_shepherd = false;
    }
	}

  // Collision
  void OnCollisionEnter(Collision collision){
    // TODO: Add rewards
    if (collision.gameObject.name == "black_sheep"){
      Debug.Log("Wolf eats black sheep");
      FindObjectOfType<ShepherdAgent>().BlackSheepDead();
      gameObject.transform.position = wolf_start_pos;
    }
    else if (collision.gameObject.name == "white_sheep"){
      Debug.Log("Wolf eats white sheep");
      FindObjectOfType<ShepherdAgent>().WhiteSheepSacrifice();
      gameObject.transform.position = wolf_start_pos;
    }
    else if (collision.gameObject.name == "shepherd"){
      Debug.Log("Wolf eats shepherd");
      FindObjectOfType<ShepherdAgent>().MySelfDead();
      gameObject.transform.position = wolf_start_pos;
    }
  }
}
