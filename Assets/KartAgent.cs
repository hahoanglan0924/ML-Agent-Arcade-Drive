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
            _checkpointManager.ResetCheckpoints();

        }

    #region Edit this region!

    //Collecting extra Information that isn't picked up by the RaycastSensors
    public override void CollectObservations(VectorSensor sensor)
    {
        //Pass the direction for the checkpoint
        Vector3 diff = _checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        if (_checkpointManager.nextCheckPointToReach != null)
            sensor.AddObservation(diff / 20); //divide 20 to normalize
    
            AddReward(-0.001f);//Promote faster driver
            sensor.AddObservation(_kartController.rb.velocity);
            sensor.AddObservation(transform.position);
        }

    //Processing the actions received
    public override void OnActionReceived(ActionBuffers actions)
    {
        ActionSegment<float> input = actions.ContinuousActions;

        if (input.Length != 3)
        {
            Debug.LogError("Unexpected number of continuous actions. Expected 2, but received " + input.Length);
            return;
        }
        
        _kartController.throttle = input[0];
        _kartController.steering = _kartController.turnInputCurve.Evaluate(input[1]) * _kartController.steerAngle;
        _kartController.jumping = input[2] != 0; // Check if this is the correct action index
       // _kartController.boosting = input[3] > 0.5f;
      //  _kartController.drift = input[4] != 0;

        // ... the rest of your code
    }
    //For manual testing with human input, the actionsOut defined here will be sent to OnActionRecieved
    public override void Heuristic(in ActionBuffers actionsOut)
        {
        ActionSegment<float> action = actionsOut.ContinuousActions;
                    action[0] = GetInput(_kartController.throttleInput);
            action[1] = GetInput(_kartController.turnInput);
            action[2] = GetInput(_kartController.jumpInput);
   //        action[3] = GetInput(_kartController.boostInput);
     //   action[4] = GetInput(_kartController.driftInput);

        }

        private float GetInput(string input)
        {
            return Input.GetAxis(input);

        }

    #endregion

}


