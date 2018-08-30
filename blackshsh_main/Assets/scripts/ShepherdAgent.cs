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

  public float speed = 10.0f;
  public float gravity = 20.0f;
  private float max_x_distance = 23.0f;
  private float max_z_distance = 17.0f;

  private CharacterController controller;

  private Vector3 shepherd_start_pos;
  private Vector3 black_sheep_start_pos;
  private Vector3 wolf_start_pos;

  private bool run_into_wall;

  public override void InitializeAgent(){
    shepherd_start_pos = gameObject.GetComponent<Transform>().position;
    black_sheep_start_pos = black_sheep.GetComponent<Transform>().position;
    wolf_start_pos = wolf.GetComponent<Transform>().position;
    controller = GetComponent<CharacterController>();
  }

  public override void CollectObservations(){
    // 2D Vectors
    //AddVectorObs(get2DPos(gameObject));
    //AddVectorObs(get2DPos(black_sheep));
    //AddVectorObs(get2DPos(white_sheep));

    // Raycast
    int layerMask = 1 << 8;
    float ray_width = 1;
    layerMask = ~layerMask;
    RaycastHit hit;
    // raycast for walls
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), ray_width, layerMask)  ||
    Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), ray_width, layerMask)         || 
    Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), ray_width, layerMask)         || 
    Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), ray_width, layerMask)){
      run_into_wall = true;
    }
    else{
      run_into_wall = false;
    }

    // wolf distances
    //AddVectorObs(Vector3.Distance(gameObject.transform.position, wolf.transform.position));
    //AddVectorObs(Vector3.Distance(black_sheep.transform.position, wolf.transform.position));
    //AddVectorObs(Vector3.Distance(white_sheep.transform.position, wolf.transform.position));
    //AddVectorObs((gameObject.transform.position.x - wolf.transform.position.x) / max_x_distance);
    //AddVectorObs((gameObject.transform.position.z - wolf.transform.position.z) / max_z_distance);
    AddVectorObs((gameObject.transform.position.x - black_sheep.transform.position.x) / max_x_distance);
    AddVectorObs((gameObject.transform.position.z - black_sheep.transform.position.z) / max_z_distance);
    
    AddVectorObs((wolf.transform.position.x - black_sheep.transform.position.x) / max_x_distance);
    AddVectorObs((wolf.transform.position.z - black_sheep.transform.position.z) / max_z_distance);

    AddVectorObs((wolf.transform.position.x - white_sheep.transform.position.x) / max_x_distance);
    AddVectorObs((wolf.transform.position.z - white_sheep.transform.position.z) / max_z_distance);

    AddVectorObs((gameObject.transform.position.x - wolf.transform.position.x) / max_x_distance);
    AddVectorObs((gameObject.transform.position.z - wolf.transform.position.z) / max_z_distance);


    AddVectorObs(wolf.GetComponent<Wolf>().follow_shepherd);
    AddVectorObs(run_into_wall);

    //AddVectorObs(wolf.transform.position);
    //AddVectorObs(white_sheep.transform.position);
    //AddVectorObs(black_sheep.transform.position);
    //AddVectorObs((wolf.transform.position - gameObject.transform.position));


  }

  public override void AgentAction(float[] vectorAction, string textAction){
    // input vectors
    if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous){
      float x = Mathf.Clamp(vectorAction[1], -1, 1);
      float z = Mathf.Clamp(vectorAction[0], -1, 1);
      float run = Mathf.Clamp(vectorAction[2], 1, 2);
      // actions
      float action_x = x * run * speed * Time.deltaTime;
      float action_z = z * run * speed * Time.deltaTime;
      // move the agent
      Vector3 movement = new Vector3(action_x, 0, action_z);
      movement.y = -gravity * Time.deltaTime;
      controller.Move(movement);
    }
    // follow reward
    if (wolf.GetComponent<Wolf>().follow_shepherd){
      SetReward(0.1f);
    }
    else if (wolf.GetComponent<Wolf>().follow_white_sheep){
      SetReward(0.5f);
    }
    else {
      SetReward(-0.1f);
    }
    // shepherd runs into the wall
    if (run_into_wall){
      SetReward(-0.1f);
    }
  }

  // Reset everything after done or time-up
  public override void AgentReset(){
    Vector3[] wolf_positions =  { new Vector3 { x = -5, y = 0, z = -5 }, 
                                  new Vector3 { x = -5, y = 0, z = 5}, 
                                  new Vector3 { x = 5, y = 0, z = -5}, 
                                  new Vector3 { x = 5, y = 0, z = 5} 
                                };
    Vector3[] shepherd_positions =  { new Vector3 { x = -2, y = 0, z = -2 }, 
                                      new Vector3 { x = -2, y = 0, z = 2}, 
                                      new Vector3 { x = 2, y = 0, z = -2}, 
                                      new Vector3 { x = 2, y = 0, z = 2} 
                                    };
    Vector3 spawnShepherd = new Vector3(  
      black_sheep_start_pos.x + shepherd_positions[Random.Range(0, 3)].x, 
      0, 
      black_sheep_start_pos.z + shepherd_positions[Random.Range(0, 3)].z);

    Vector3 spawnWolf = new Vector3( 
      black_sheep_start_pos.x + wolf_positions[Random.Range(0, 3)].x, 
      0, 
      black_sheep_start_pos.z + wolf_positions[Random.Range(0, 3)].z);

    wolf.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    wolf.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
    wolf.GetComponent<Transform>().position = spawnWolf;
    wolf.GetComponent<Transform>().rotation = Random.rotation;
    black_sheep.GetComponent<Transform>().position = black_sheep_start_pos;
    gameObject.transform.position = spawnShepherd;
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

}
