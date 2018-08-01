using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ShepherdAgent : Agent {

  [Header("Shepherd Specific")]
  public GameObject wolf;
  public GameObject white_sheep;
  public GameObject black_sheep;
  public GameObject walls;

  public float speed = 10.0F;
  public float gravity = 20.0F;

  private CharacterController controller;

  private Vector3 shepherd_start_pos;
  private Vector3 black_sheep_start_pos;
  private Vector3 wolf_start_pos;
  private Quaternion wolf_start_rot;

  public override void InitializeAgent(){
    shepherd_start_pos = gameObject.GetComponent<Transform>().position;
    black_sheep_start_pos = black_sheep.GetComponent<Transform>().position;
    wolf_start_pos = wolf.GetComponent<Transform>().position;
    wolf_start_rot = wolf.GetComponent<Transform>().rotation;
    controller = GetComponent<CharacterController>();
  }

  public override void CollectObservations(){
    // 2D Vectors
    //AddVectorObs(get2DPos(gameObject));
    //AddVectorObs(get2DPos(black_sheep));
    //AddVectorObs(get2DPos(white_sheep));

    // Scalars
    // Walls
    Transform[] wall_children = walls.GetComponentsInChildren<Transform>();
    float min_wall_dist = 20f;
    foreach (Transform wall in wall_children) {
       // whatever
      float wall_dist = Vector3.Distance(gameObject.transform.position, wall.position);
      if (wall_dist < min_wall_dist){
        min_wall_dist = wall_dist;
      }
    }
    Debug.Log("wall: " + min_wall_dist);
    AddVectorObs(min_wall_dist);

    // wolf
    AddVectorObs(Vector3.Distance(gameObject.transform.position, wolf.transform.position));
    AddVectorObs(Vector3.Distance(black_sheep.transform.position, wolf.transform.position));
    AddVectorObs(Vector3.Distance(white_sheep.transform.position, wolf.transform.position));
    AddVectorObs(wolf.GetComponent<Wolf>().follow_shepherd);

    //AddVectorObs(wolf.transform.position);
    //AddVectorObs(white_sheep.transform.position);
    //AddVectorObs(black_sheep.transform.position);
    //AddVectorObs((wolf.transform.position - gameObject.transform.position));


  }

  public override void AgentAction(float[] vectorAction, string textAction){
    // input vectors
    if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous){
      float x = Mathf.Clamp(vectorAction[0], -1, 1);
      float z = Mathf.Clamp(vectorAction[1], -1, 1);
      float run = Mathf.Clamp(vectorAction[2], 1, 2);
      // actions
      float action_x = x * run * speed * Time.deltaTime;
      float action_z = z * run * speed * Time.deltaTime;

      // actual distance
      //float actual_dist_wolf = Vector3.Distance(gameObject.transform.position, wolf.transform.position);
      //float actual_dist_white = Vector3.Distance(gameObject.transform.position, white_sheep.transform.position);

      // move the agent
      Vector3 movement = new Vector3(action_x, 0, action_z);
      movement.y = -gravity * Time.deltaTime;
      controller.Move(movement);

      /*
      if (action_x != 0 || action_z != 0){
        //gameObject.transform.Translate(action_x, 0, action_z); 
        float new_dist_wolf = Vector3.Distance(gameObject.transform.position, wolf.transform.position);
        float new_dist_white = Vector3.Distance(gameObject.transform.position, white_sheep.transform.position);
        // move to the direction of wolf

        if (!wolf.GetComponent<Wolf>().follow_shepherd){
          if (actual_dist_wolf < new_dist_wolf){
            SetReward(0.1f);
          }
          else {
            SetReward(-0.1f);
          }
        }
        // move with wolf to white sheep
        else{
          if (actual_dist_white < new_dist_white){
            SetReward(0.1f);
          }
          else {
            SetReward(-0.1f);
          }
        }
      }
      */
    }

    // follow reward
    if (wolf.GetComponent<Wolf>().follow_shepherd){
      SetReward(0.1f);
    }
    else {
      SetReward(-0.01f);
    }
  }

  // Reset everything after done or time-up
  public override void AgentReset(){
    gameObject.transform.position = shepherd_start_pos;
    black_sheep.GetComponent<Transform>().position = black_sheep_start_pos;
    wolf.GetComponent<Transform>().position = wolf_start_pos;
    wolf.GetComponent<Transform>().rotation = wolf_start_rot;
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

  // get 2D Observation instead of 3D
  public Vector2 get2DPos(GameObject obj){
    Vector2 new_pos;
    new_pos.x = obj.transform.position.x;
    new_pos.y = obj.transform.position.z;
    return new_pos;
  }

  // Collider
  /*
  public void OnControllerColliderHit(ControllerColliderHit hit) {
    Rigidbody body = hit.collider.attachedRigidbody;
    Debug.Log("hit something");
    if (body == null || body.isKinematic)
        return;
  }
  */
}
