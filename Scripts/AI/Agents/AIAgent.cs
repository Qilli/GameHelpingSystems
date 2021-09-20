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

    public class KinematicData
    {
        public Vector3 position;
        public Vector3 velocity;
        public float orientation;
        public float rotation;

        public void set(Vector3 position_,Vector3 velocity_,float orientation_,float rotation_)
        {
            position = position_;
            velocity = velocity_;
            orientation = orientation_;
            rotation = rotation_;
        }

        public static float DirectionToOrientation(Vector3 direction)
        {
            return Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg;
        }

        public static Vector3 OrientationToDirection(float orientation)    
        {
            return new Vector3(Mathf.Sin(orientation*Mathf.Deg2Rad), 0, Mathf.Cos(orientation * Mathf.Deg2Rad));
        }
    }
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AIAttackSystem))]
    public class AIAgent : Base.ObjectsControl.BaseObject
    {

        private KinematicData agentKinematicData = new KinematicData();
        private Rigidbody rigid;
        public KinematicData AgentKinematics
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
                agentKinematicData.set(transform.position, Vector3.zero, KinematicData.DirectionToOrientation(transform.forward), 0);
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
            transform.localEulerAngles = new Vector3(0, agentKinematicData.orientation, 0);
            transform.Translate(agentKinematicData.velocity*Time.deltaTime,Space.World);     
            agentKinematicData.position = transform.position;
        }
        public void setInstantlyOrientation(float orientation)
        {
            agentKinematicData.orientation=orientation;
            transform.localEulerAngles = new Vector3(0, agentKinematicData.orientation, 0);
        }
        public override void onUpdate(float delta)
        {
            if(!agentParams.moveByNavMesh)
            {
            updateSteering();
            }
            else
            {
            agentKinematicData.position = transform.position; 
            }
        }

    }

}
