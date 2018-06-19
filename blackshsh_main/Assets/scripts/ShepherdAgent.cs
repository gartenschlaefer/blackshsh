using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShepherdAgent : Agent {

  [Header("Specific to Wolf")]
  public GameObject wolf;

  private Vector3 shepherd_start_pos;

  public override void InitializeAgent(){
    shepherd_start_pos = gameObject.GetComponent<Transform>().position;
  }

  public override void CollectObservations(){
    /*
    AddVectorObs(gameObject.transform.rotation.z);
    AddVectorObs(gameObject.transform.rotation.x);
    AddVectorObs((wolf.transform.position - gameObject.transform.position));
    SetTextObs("Testing " + gameObject.GetInstanceID());
    */
  }

  public override void AgentAction(float[] vectorAction, string textAction){

  }

  public override void AgentReset(){
    gameObject.transform.position = shepherd_start_pos;
  }
}
