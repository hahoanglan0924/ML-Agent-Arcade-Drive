using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using System;

    public class KartAgent : Agent
    {
    
        public CheckpointManager _checkpointManager;
        private WheelVehicle _kartController;

        //called once at the start
        public override void Initialize()
        {
            _kartController = GetComponent<WheelVehicle>();
       
    }

        //Called each time it has timed-out or has reached the goal
        public override void OnEpisodeBegin()
        {
        _checkpointManager.ResetCheckpoint(transform);
        

        }

       #region Edit this region!

    //Collecting extra Information that isn't picked up by the RaycastSensors
    public override void CollectObservations(VectorSensor sensor)
    {

        Vector3 checkpointForward = _checkpointManager.GetNextCheckpoint(transform);
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);


            AddReward(-0.001f);//Promote faster driver
            sensor.AddObservation(_kartController.rb.velocity);
           
        }

    //Processing the actions received
    public override void OnActionReceived(ActionBuffers actions)
    {

        float forwardAmount = 0f;
        float turnAmount = 0f;
        float jump = 0;   
        forwardAmount = actions.ContinuousActions[0];
        turnAmount = actions.ContinuousActions[1];
      //  jump = actions.ContinuousActions[2];
        
        
        _kartController.throttle = forwardAmount;
        _kartController.steering = _kartController.turnInputCurve.Evaluate(turnAmount) * _kartController.steerAngle;
     //   _kartController.jumping = (jump > 0.5f) ? true : false;
        // _kartController.boosting = input[3] > 0.5f;
        //  _kartController.drift = input[4] != 0;

        // ... the rest of your code
    }
    //For manual testing with human input, the actionsOut defined here will be sent to OnActionRecieved
    public override void Heuristic(in ActionBuffers actionsOut)
        {
        ActionSegment<float> input = actionsOut.ContinuousActions;
        float forwardAction = 0;
        float turnAction = 0;
        float jump = 0;
        forwardAction = GetInput(_kartController.throttleInput);
        turnAction = GetInput(_kartController.turnInput);
      //  jump = Input.GetKey(KeyCode.Space) ? 1 : 0;
        //        action[3] = GetInput(_kartController.boostInput);
        //   action[4] = GetInput(_kartController.driftInput);
        input[0] = forwardAction;
        input[1] = turnAction;
     //   input[2] = jump;

    }

        private float GetInput(string input)
        {
            return Input.GetAxis(input);

        }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-0.5f);
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            AddReward(-0.1f);
        }

    }

    #endregion

}


