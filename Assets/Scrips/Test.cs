using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Test : Agent
{
    public override void Initialize() // void Start() ��� ����ϴ� �Լ�
    {
        
    }

    public override void OnEpisodeBegin() // ������ ���(�ݹ��Լ�)
    {
        // �ᱹ �÷��̾ ��¼��� ����Ͽ� �÷��̾ ��Ȱ��ȭ �ϸ� �ٽ� ó������ �����Ѵ�.
    }

    public override void OnActionReceived(ActionBuffers actions) // void Update() ��� ����ϴ� �Լ�
    {
        
    }

    public override void CollectObservations(VectorSensor sensor) // 
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut) // �׽�Ʈ�� ���� ����� ���� action ���� �������ִ� ���
    {
        
    }
}
