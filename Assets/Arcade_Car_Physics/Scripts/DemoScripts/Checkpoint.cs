using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Checkpoint : MonoBehaviour
    {
    private CheckpointManager checkpointManager;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CheckpointManager>() != null)
            {
            checkpointManager.CarThroughCheckpoint(this, other.transform);
            }
        }

    public void SetTrackCheckpoints(CheckpointManager trackChechpoints)
    {
        checkpointManager = trackChechpoints;
    }
    }
