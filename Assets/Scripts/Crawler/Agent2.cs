using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(JointDriveController2))] // Required to set joint forces
public class Agent2 : MonoBehaviour
{
    public EnvironmentManager environmentManager;
    private Client client;
    public int decisionPeriod = 5;
    public List<double> currentStateData;
    public double m_Reward;
    public bool done;
    public int decisionStep = 0;
    public Action stepCallBack;
    protected JointDriveController2 jdController;
    public bool freezeBody = false;
    public Transform stabilizingPivot;
    public Transform targetTransform;
    public Transform topHierarchyBodyPart;
    public bool trainingEnvironment = true;
    public bool testingModel = false;
    public bool isModelAwake = false;


    //This will be used as a stabilized model space reference point for observations
    //Because ragdolls can move erratically during training, using a stabilized reference transform improves learning
    public OrientationCubeController2 orientationCube;


    private void Awake()
    {
        Initialize();
    }

    public void AwakeModel(bool isAwake)
    {
        client.receiveMessage = isAwake;
        isModelAwake = isAwake;

        if(isAwake)
            client.StartClient();
    }

    protected virtual void Initialize()
    {
        jdController = GetComponent<JointDriveController2>();
        currentStateData = new List<double>();
        orientationCube.Initialize(stabilizingPivot);
        client= GetComponent<Client>();

        AwakeModel(isModelAwake);
        //StartCoroutine(ss());
    }

    public virtual void OnEpisodeBegin()
    {
        done = false;
        freezeBody = false;
        decisionStep = 0;
        foreach (var bodyPart in jdController.bodyPartsDict.Values)
        {
            bodyPart.Reset(bodyPart);
        }

        if(environmentManager!=null)
            environmentManager.InitializeEnvironmentRandomly();
        RandomlyRotateObjBeforeEpisode();
        SpawnTarget();
        orientationCube.UpdateOrientation(stabilizingPivot, targetTransform);
    }

    public virtual void RandomlyRotateObjBeforeEpisode()
    {

    }

    public virtual void CollectObservations()
    {

    }

    public virtual void ActionReceived(List<double> actionBuffers)
    {

    }

    protected virtual void SpawnTarget()
    {

    }

    public virtual void EndEpisode()
    {
        if (trainingEnvironment == false)
            return;

        done = true;
        freezeBody = true;
        decisionStep = 0;
        if (stepCallBack != null)
        {
            stepCallBack();
        }
    }

    public void SetReward(float reward)
    {
        //Utilities.DebugCheckNanAndInfinity(reward, "reward", "SetReward");
        //m_CumulativeReward += reward - m_Reward;
        m_Reward = reward;
        //m_Reward += reward;
    }

    public void AddReward(float increment)
    {
        //Utilities.DebugCheckNanAndInfinity(increment, "increment", "AddReward");
        m_Reward += increment;
        //m_CumulativeReward += increment;
    }

    protected void UpdateOrientationObjects()
    {
        orientationCube.UpdateOrientation(stabilizingPivot, targetTransform);
    }

    public virtual void FreezeRigidBody(bool freeze)
    {
        if (targetTransform != null)
            targetTransform.GetComponent<TargetBase>().FreezeRigidBody(freeze);

        for (int i = 0; i < jdController.bodyPartsList.Count; i++)  
        {
            if (freeze)
            {
                freezeBody = true;
                if (jdController.bodyPartsList[i].isAlreadyFroozen == false)
                {
                    jdController.bodyPartsList[i].isAlreadyFroozen = true;
                    jdController.bodyPartsList[i].SaveVelocity();
                    jdController.bodyPartsList[i].rb.constraints = RigidbodyConstraints.FreezeAll;
                    jdController.bodyPartsList[i].rb.isKinematic = true;
                }
            }
            else
            {
                freezeBody = false;
                if (jdController.bodyPartsList[i].isAlreadyFroozen == true)
                {
                    jdController.bodyPartsList[i].isAlreadyFroozen = false;
                    jdController.bodyPartsList[i].rb.isKinematic = false;
                    jdController.bodyPartsList[i].rb.constraints = jdController.bodyPartsList[i].rbInitialConstraints;
                    jdController.bodyPartsList[i].LoadSavedVelocity();
                }
            }
        }
    }
}
