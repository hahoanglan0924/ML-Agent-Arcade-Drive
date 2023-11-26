using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;



public class CheckpointManager : MonoBehaviour
{
    public Transform checkpoints;
    [SerializeField]private List<Checkpoint> checkpointList;
    private List<int> nextCheckpointSingleIndex;
    [SerializeField]private List<Transform> carTransformList;

    private void Awake()
    {
        checkpointList = new List<Checkpoint>();
        KartAgent[] cars = FindObjectsOfType<KartAgent>();
        foreach (KartAgent car in cars)
        {
            carTransformList.Add(car.transform);
        }
        foreach (Transform checkpointSingleTransform in checkpoints)
        {
            Checkpoint checkPointSingle = checkpointSingleTransform.GetComponent<Checkpoint>();
            checkPointSingle.SetTrackCheckpoints(this);
            checkpointList.Add(checkPointSingle);
        }
        nextCheckpointSingleIndex = new List<int>();
        foreach(Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndex.Add(0);
        }
       
    }

    public void CarThroughCheckpoint(Checkpoint checkpoint, Transform carTransfrom)
    {
        int nextCheckpointIndex = nextCheckpointSingleIndex[carTransformList.IndexOf(carTransfrom)];
        KartAgent kartAgent = carssTransfrom.GetComponent<KartAgent>();
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex) {
            //Correct checkpoint

            Checkpoint correctCheckpointSingle = checkpointList[nextCheckpointIndex];

            nextCheckpointSingleIndex[carTransformList.IndexOf(carTransfrom)] = (nextCheckpointIndex + 1) % checkpointList.Count;
         
            
            kartAgent.AddReward(1f); // For example, decrease reward by 1
        }
        else
        {
            //Wrong checkpoint
            Debug.Log("Wrong point" + carTransfrom.gameObject.name);
                       kartAgent.AddReward(-1f); // For example, decrease reward by 1
        }
    }

    public Vector3 GetNextCheckpoint(Transform carTransform)
    {
        int nextCheckpointIndex = nextCheckpointSingleIndex[carTransformList.IndexOf(carTransform)];
        Transform nextCheckPoint = checkpointList[nextCheckpointIndex].GetComponent<Transform>();

        return nextCheckPoint.forward;
    }

    public void ResetCheckpoint(Transform carTransform)
    {
        nextCheckpointSingleIndex[carTransformList.IndexOf(carTransform)] = 0;
    }


}