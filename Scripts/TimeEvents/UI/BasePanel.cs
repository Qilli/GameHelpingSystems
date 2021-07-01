using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.UI
{
    public class BasePanel : MonoBehaviour, IUIOnOffElement
    {
        #region PARAMS
        private List<IUIOnOffElement> childOnOffElements= new List<IUIOnOffElement>();
        #endregion

        #region PRIVATE FUNC
        protected virtual void Awake()
        {
           GetComponentsInChildren<IUIOnOffElement>(false,childOnOffElements);
           childOnOffElements.Remove(this);    
        }
        private void turnChildren(bool on)
        {
            if(on)
            {
                foreach (IUIOnOffElement e in childOnOffElements) e.turnOnElement();
            }else
            {
                foreach (IUIOnOffElement e in childOnOffElements) e.turnOffElement();
            }
        }
        private void turnChildrenInstantly(bool on)
        {
            if (on)
            {
                foreach (IUIOnOffElement e in childOnOffElements) e.turnOnElementInstantly();
            }
            else
            {
                foreach (IUIOnOffElement e in childOnOffElements) e.turnOffElementInstantly();
            }
        }
        #endregion
        #region PUBLIC FUNC
        public virtual void turnOnElement()
        {
            turnChildren(true);
        }
        public virtual void turnOffElement()
        {
            turnChildren(false);
        }
        public virtual void turnOffElementInstantly()
        {
            turnChildrenInstantly(false);
        }
        public virtual void turnOnElementInstantly()
        {
            turnChildrenInstantly(true);
        } 
        #endregion
    }
}
