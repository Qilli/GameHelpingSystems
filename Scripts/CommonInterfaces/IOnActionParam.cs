using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.CommonInterfaces
{
    public interface IOnActionParam<T>
    {
        public void onAction(T param);
    }

}
