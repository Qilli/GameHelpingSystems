//K.Homa 27.02.2021
using UnityEngine;
using Base.Audio;
namespace Base
{

    public class GlobalDataContainer : MonoBehaviour
    {
        //objects
        [Header("Game Systems")]
        public Cameras.CamerasManager camerasMgr;
        public AudioController audioController;
        public Base.AI.Behaviours.BehaviourTreesManager behaviourTreesMgr;
        public GameSystem gameSystem;      
        private static GlobalDataContainer container = null;

        public static GlobalDataContainer It
        {
            get
            {
                if (container == null)
                {
                    container = GameObject.FindObjectOfType<GlobalDataContainer>();
                }

#if UNITY_EDITOR

                if (container == null)
                {
                    Base.Log.Logging.Log("Wrong initialization order!");
                    Debug.DebugBreak();
                }

#endif
                return container;
            }
        }

        protected void Awake()
        {
            container = this;
        }

    }
}
