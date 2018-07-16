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
  private Vector3 black_sheep_start_pos;

  public override void InitializeAgent(){
    shepherd_start_pos = gameObject.GetComponent<Transform>().position;
    black_sheep_start_pos = black_sheep.GetComponent<Transform>().position;
  }

  public override void CollectObservations(){
    AddVectorObs(gameObject.transform.position);
    AddVectorObs(wolf.transform.position);
    AddVectorObs(white_sheep.transform.position);
    AddVectorObs(black_sheep.transform.position);
    AddVectorObs((wolf.transform.position - gameObject.transform.position));
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

      // actual distance
      float actual_dist = Vector3.Distance(gameObject.transform.position, wolf.transform.position);

      // move the agent
      if (action_x != 0 || action_z != 0){
        gameObject.transform.Translate(action_x, 0, action_z); 
        float new_dist = Vector3.Distance(gameObject.transform.position, wolf.transform.position);
        // move to the direction of wolf
        if (!wolf.GetComponent<Wolf>().follow_shepherd){
          if (actual_dist < new_dist){
            SetReward(0.1f);
          }
          else {
            SetReward(-0.1f);
          }
        }
      }
    }
    if (wolf.GetComponent<Wolf>().follow_shepherd){
      SetReward(0.1f);
    }
    else {
      SetReward(0.01f);
    }
  }

  public override void AgentReset(){
    gameObject.transform.position = shepherd_start_pos;
    black_sheep.GetComponent<Transform>().position = black_sheep_start_pos;
  }

  // feedback
  public void BlackSheepDead(){
    SetReward(-10f);
    Done();
  }

  public void MySelfDead(){
    SetReward(-5f);
    Done();
  }

  public void WhiteSheepSacrifice(){
    SetReward(10f);
    Done();
  }
}
