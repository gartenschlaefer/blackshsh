using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShepherdAgent : Agent {

  [Header("Specific to Wolf")]
  public GameObject wolf;

  public override void InitializeAgent(){

  }

  public override void CollectObservations(){
    AddVectorObs(gameObject.transform.rotation.z);
    AddVectorObs(gameObject.transform.rotation.x);
    AddVectorObs((wolf.transform.position - gameObject.transform.position));
    AddVectorObs(wolf.transform.GetComponent<Rigidbody>().velocity);
    SetTextObs("Testing " + gameObject.GetInstanceID());
  }

  public override void AgentAction(float[] vectorAction, string textAction){

  }

  public override void AgentReset(){

  }
}
