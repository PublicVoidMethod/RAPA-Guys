using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Test : Agent
{
    public override void Initialize() // void Start() 대신 사용하는 함수
    {
        
    }

    public override void OnEpisodeBegin() // 리셋을 담당(콜백함수)
    {
        // 결국 플레이어가 결승선을 통과하여 플레이어가 비활성화 하면 다시 처음부터 시작한다.
    }

    public override void OnActionReceived(ActionBuffers actions) // void Update() 대신 사용하는 함수
    {
        
    }

    public override void CollectObservations(VectorSensor sensor) // 
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut) // 테스트를 위해 사람이 직접 action 값을 전달해주는 기능
    {
        
    }
}
