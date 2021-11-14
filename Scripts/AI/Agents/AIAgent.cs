using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Agents
{

    public class SteeringOutput
    {
        public Vector3 velocity;
        public Vector3 rotation;
    }

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AIAttackSystem))]
    public class AIAgent : Base.ObjectsControl.BaseObject
    {

        private Physics.KinematicAgentData agentKinematicData = new Physics.KinematicAgentData();
        private Rigidbody rigid;
        public Physics.KinematicAgentData AgentKinematics
        {
            get => agentKinematicData;
        }
        //gameplaySystem
        public AIAttackSystem attackSystem;
        //navmesh agent
        public UnityEngine.AI.NavMeshAgent NavAgent { get; private set; } = null;
        private AgentParameters agentParams;
        public override void init()
        {
            base.init();
            if(!inited)
            {
                rigid = GetComponent<Rigidbody>();
                NavAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                agentParams= GetComponent<AgentParameters>();
                agentKinematicData.set(transform.position, Vector3.zero, Physics.KinematicAgentData.DirectionToOrientation(transform.forward), 0);
                inited = true;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            init();
        }
        public virtual void updateSteering()
        {
            transform.localEulerAngles = new Vector3(0, agentKinematicData.OrientationHorizontal, 0);
            transform.Translate(agentKinematicData.Velocity*Time.deltaTime,Space.World);     
            agentKinematicData.Position = transform.position;
        }
        public void setInstantlyOrientation(float orientation)
        {
            agentKinematicData.OrientationHorizontal=orientation;
            transform.localEulerAngles = new Vector3(0, agentKinematicData.OrientationHorizontal, 0);
        }
        public override void onUpdate(float delta)
        {
            if(!agentParams.moveByNavMesh)
            {
            updateSteering();
            }
            else
            {
            agentKinematicData.Position = transform.position; 
            }
        }

    }

}
