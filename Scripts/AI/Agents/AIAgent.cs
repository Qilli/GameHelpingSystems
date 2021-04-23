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
            return Mathf.Atan2(-direction.x, direction.z);
        }

        public static Vector3 OrientationToDirection(float orientation)
        {
            return new Vector3(Mathf.Deg2Rad * orientation, 0, Mathf.Deg2Rad * orientation);
        }
    }
    [RequireComponent(typeof(Rigidbody))]
    public class AIAgent : Base.ObjectsControl.BaseObject
    {

        private KinematicData agentKinematicData = new KinematicData();
        private Rigidbody rigid;
        public KinematicData AgentKinematics
        {
            get => agentKinematicData;
        }
        public override void init()
        {
            base.init();
            if(!inited)
            {
                rigid = GetComponent<Rigidbody>();
                agentKinematicData.set(transform.position, Vector3.zero, KinematicData.DirectionToOrientation(transform.forward), 0);
                inited = true;
            }
        }

        public virtual void updateSteering()
        {
            rigid.velocity = agentKinematicData.velocity;
            transform.localEulerAngles = KinematicData.OrientationToDirection(agentKinematicData.orientation);
        }

        public override void onFixedUpdate(float fixedDelta)
        {
            base.onFixedUpdate(fixedDelta);
            updateSteering();
        }

    }

}
