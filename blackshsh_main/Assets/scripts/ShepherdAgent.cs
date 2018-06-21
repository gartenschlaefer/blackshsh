using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ShepherdAgent : Agent {

  [Header("Shepherd Specific")]
  public GameObject wolf;
  public GameObject white_sheep;
  public GameObject black_sheep;

  public float speed;

  private Vector3 shepherd_start_pos;

  public override void InitializeAgent(){
    shepherd_start_pos = gameObject.GetComponent<Transform>().position;
  }

  public override void CollectObservations(){
    AddVectorObs(gameObject.transform.position);
    AddVectorObs(wolf.transform.position);
    AddVectorObs(white_sheep.transform.position);
    AddVectorObs(black_sheep.transform.position);
    //AddVectorObs((wolf.transform.position - gameObject.transform.position));
    //Debug.Log("Collect");
  }

  public override void AgentAction(float[] vectorAction, string textAction){
    // input vectors
    if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous){
      float x = Mathf.Clamp(vectorAction[0], -1, 1);
      float z = Mathf.Clamp(vectorAction[1], -1, 1);
      float run = Mathf.Clamp(vectorAction[2], 1, 2);
      // actions
      float action_x = x * speed * run;
      float action_z = z * speed * run;
      // move the agent
      if (action_x != 0 || action_z != 0){
        Debug.Log("Translate shepherd");
        gameObject.transform.Translate(action_x, 0, action_z); 
      }
    }
  }

  public override void AgentReset(){
    gameObject.transform.position = shepherd_start_pos;
  }
}
