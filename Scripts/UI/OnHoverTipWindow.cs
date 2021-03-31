//K.Homa 27.02.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Base.Log;
namespace Base.UI
{

    public class OnHoverTipWindow : MonoBehaviour
    {
        public GameObject paramsContainer;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI textDesc;
        public TextMeshProUGUI[] paramsList;

        public void disableObject()
        {
            foreach (TextMeshProUGUI t in paramsList)
            {
                t.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }

        public void enableObject()
        {
            gameObject.SetActive(true);
        }

        public void setPosition(Vector2 p)
        {
            transform.position = p;
        }

        public void setTitle(string title, Color c)
        {
            titleText.text = title;
            titleText.color = c;
        }

        public void setDesc(string desc, Color c)
        {
            textDesc.text = desc;
            textDesc.color = c;
        }

        public void resetParams()
        {
            if (paramsContainer != null)
            {
                paramsContainer.SetActive(true);
            }
            foreach (TextMeshProUGUI t in paramsList)
            {
                t.gameObject.SetActive(false);
            }
        }

        public void hideParamsContainer()
        {
            if (paramsContainer == null)
            {
                Logging.Log("Params container not set", Base.Log.BaseLogType.ERROR);
            }
            else
            {
                paramsContainer.SetActive(false);
            }
        }

        public void setParam(int index, string text, Color c)
        {
            paramsList[index].gameObject.SetActive(true);
            paramsList[index].text = text;
            paramsList[index].color = c;
        }

    }

}
