using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Test : Agent
{
    //void Start()
    //{
        
    //}

    public override void Initialize() // void Start() 대신 사용하는 함수
    {
        
    }

    public override void OnEpisodeBegin() // 한 판이 시작될 때 호출 함수??????????????????
    {
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut) // 테스트를 위해 사람이 직접 action 값을 주는 기능
    {
        
    }

    void Update()
    {
        
    }
}
