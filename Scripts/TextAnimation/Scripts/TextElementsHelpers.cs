using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Base.TextControl
{
    public class TextElementsHelpers
    {
        public static TextElement create3DTextPopup(Transform parent,Vector3 localOffset,
            string text,float fontSize,float lifeTime,Color color,float popupSpeed)
        {
            TextElementsManager mgr = Base.GlobalDataContainer.It.textsElementsManager;
            //create text object
            TextElement element = mgr.CreateTextElement(TextType.Text3D, text, parent.position, color,
                fontSize, lifeTime);
            //set as parent
            element.setParent(parent);
            element.setLocalOffset(localOffset);
            //add animation for start and end
            TextAnimationScale animScaleShow = new TextAnimationScale(popupSpeed,Vector3.zero,Vector3.one);
            element.AddOnShowAnimation(animScaleShow);
            TextAnimationScale animScaleHide = new TextAnimationScale(popupSpeed, Vector3.one, Vector3.zero);
            element.AddOnHideAnimation(animScaleHide);
            return element;
        }
    }
}
