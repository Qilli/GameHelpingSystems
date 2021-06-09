using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.AI.Behaviours
{
    [System.Serializable]
    public class Blackboard: ISerializationCallbackReceiver
    {
        [SerializeField]
        private Dictionary<string, SharedVariable> variables = new Dictionary<string, SharedVariable>();
        [SerializeField]
        private BilboardSerializer serializer=new BilboardSerializer();
        /// <summary>
        /// return copy of entire bilboard object
        /// </summary>
        /// <returns></returns>
        public Blackboard getCopy()
        {
            Blackboard b = new Blackboard();
            foreach(KeyValuePair<string,SharedVariable> elem in variables)
            {
                b.variables.Add(elem.Key,elem.Value.getCopy());
            }
            return b;
        }
        public SharedVariable getVariableByName(string name_)
        {
            if(alreadyContains(name_))
            {
                return variables[name_];
            }return null;
        }

        public string[] getNamesOfVariablesByType(SharedVariable.SharedType type)
        {
            List<string> result = new List<string>();
            foreach(KeyValuePair<string,SharedVariable> value in variables)
            {
                if (value.Value.type == type) result.Add(value.Key);
            }
            return result.ToArray();
        }

        public bool addNewVariable(string name,SharedVariable variable)
        {
            if(!alreadyContains(name))
            {
                variable.name = name;
                variables.Add(name, variable);
                return true;
            }
            return false;
        }
        public bool removeVariableByName(string name)
        {
            if(alreadyContains(name))
            {
                variables.Remove(name);
                return true;
            }return false;
        }
        public bool alreadyContains(string name)
        {
            return variables.ContainsKey(name);
        }
        public IEnumerable<SharedVariable> getAllSharedVariables()
        {
            foreach(KeyValuePair<string,SharedVariable> sv in variables)
            {
                yield return sv.Value;
            }
        }

        public void OnBeforeSerialize()
        {
            Validate();
            serializer.clear();
            foreach(KeyValuePair<string,SharedVariable> value in variables)
            {
                serializer.names.Add(value.Key);
                addValueToSerializer(value.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            variables.Clear();
            foreach(string name in serializer.names)
            {
                variables.Add(name, getSharedVariableFromSerializer(name));
            }
        }

        private SharedVariable getSharedVariableFromSerializer(string name)
        {
            SharedVariable result = null;
            result = serializer.sharedFloats.Find(x => x.name == name);
            if (result != null) return result;
            result = serializer.sharedGameObjects.Find(x => x.name == name);
            if (result != null) return result;
            result = serializer.sharedInts.Find(x => x.name == name);
            if (result != null) return result;
            result = serializer.sharedStrings.Find(x => x.name == name);
            if (result != null) return result;
            result = serializer.sharedTransforms.Find(x => x.name == name);
            if (result != null) return result;
            result = serializer.sharedObjects.Find(x => x.name == name);
            if (result != null) return result;
            result = serializer.sharedBools.Find(x => x.name == name);
            if (result != null) return result;
            result = serializer.sharedVectors.Find(x => x.name == name);
            if (result != null) return result;
            return result;
        }

        private void addValueToSerializer(SharedVariable sv)
        {
            if (sv.type == SharedVariable.SharedType.FLOAT) serializer.sharedFloats.Add((SharedFloat)sv);
            else if (sv.type == SharedVariable.SharedType.GAMEOBJECT) serializer.sharedGameObjects.Add((SharedGameObject)sv);
            else if (sv.type == SharedVariable.SharedType.OBJECT) serializer.sharedObjects.Add((SharedObject)sv);
            else if (sv.type == SharedVariable.SharedType.INT) serializer.sharedInts.Add((SharedInt)sv);
            else if (sv.type == SharedVariable.SharedType.STRING) serializer.sharedStrings.Add((SharedString)sv);
            else if (sv.type == SharedVariable.SharedType.TRANSFORM) serializer.sharedTransforms.Add((SharedTransform)sv);
            else if (sv.type == SharedVariable.SharedType.BOOL) serializer.sharedBools.Add((SharedBool)sv);
            else if (sv.type == SharedVariable.SharedType.VECTOR) serializer.sharedVectors.Add((SharedVector)sv);
        }

        private void Validate()
        {
            List<string> toErase = new List<string>();
            foreach (KeyValuePair<string, SharedVariable> value in variables)
            {
                if(value.Value==null)
                {
                    toErase.Add(value.Key);
                }
            }
            for(int a=0;a<toErase.Count;++a)
            {
                variables.Remove(toErase[a]);
            }
        }
    }
    [System.Serializable]
    public class BilboardSerializer
    {
        public List<string> names = new List<string>();
        public List<SharedInt> sharedInts = new List<SharedInt>();
        public List<SharedTransform> sharedTransforms = new List<SharedTransform>();
        public List<SharedFloat> sharedFloats = new List<SharedFloat>();
        public List<SharedString> sharedStrings = new List<SharedString>();
        public List<SharedGameObject> sharedGameObjects = new List<SharedGameObject>();
        public List<SharedObject> sharedObjects = new List<SharedObject>();
        public List<SharedBool> sharedBools = new List<SharedBool>();
        public List<SharedVector> sharedVectors = new List<SharedVector>();

        public void clear()
        {
            names.Clear();
            sharedInts.Clear();
            sharedTransforms.Clear();
            sharedFloats.Clear();
            sharedStrings.Clear();
            sharedGameObjects.Clear();
            sharedBools.Clear();
            sharedObjects.Clear();
            sharedVectors.Clear();
        }
    }
}
