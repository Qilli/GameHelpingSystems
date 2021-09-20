

using System.Collections.Generic;
using UnityEngine;
namespace Base
{
    public class ImportantObjectsContainer : Base.ObjectsControl.BaseObject
    {
        [Header("Tags to find")]
        public List<string> tagsToFind;
        [Header("Names to find")]
        public List<string> namesToFind;
        private List<(string tag, Transform transform)> tagsObjects = new List<(string, Transform)>();
        private List<(string name, Transform transform)> namesObjects = new List<(string, Transform)>();
        public override void init()
        {
            base.init();
            //find once important objects by tag
            tagsToFind.ForEach((elem)=> tagsObjects.Add((elem,GameObject.FindGameObjectWithTag(elem)?.transform)));
            //find by name
            namesToFind.ForEach((elem)=> namesObjects.Add((elem,GameObject.Find(elem)?.transform)));         
        }
        public Transform getByName(string name)
        {
            var result =namesObjects.Find((elem)=>elem.name == name);
            return result.name!=""?result.transform:null;
        }
        public Transform getByTag(string tag)
        {
            var result =tagsObjects.Find((elem)=>elem.tag == tag);
            return result.tag!=""?result.transform:null;
        }
    }
}
