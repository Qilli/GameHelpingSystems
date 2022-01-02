//K.Homa 02.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Base.ObjectsControl;

namespace Base.Cameras
{
    public class SimpleCamera2D : BaseObject
    {
        [Header("General")]
        public Camera2DSettings settingsCamera;
        public CameraMoveTo moveToControl;
        public Camera usedCamera;
        public float targetCamSizeOnZoom = 4;
        public bool isCameraPositionLocked = false;
        public bool isPanByMiddleMouseOn = false;
        public float panScreenEdgePercentage = 0.1f;
        private float zoom = 0;
        private Vector3 cameraPosition;
        private Vector3 currentCameraTarget;
        private float currentCameraOrthoTarget;
        private Transform cameraTransform;

        //drag
        private bool isDraging = false;
        private int scrolling = 0;
        private Vector3 mouseOffset;
        private Vector2 panOffset;


        private Vector3 bottomLeftConst = new Vector3(0, 0, 0);
        private Vector3 bottomRightConst = new Vector3(1, 0, 0);
        private Vector3 topLeftConst = new Vector3(0, 1, 0);
        private Vector3 topRightConst = new Vector3(1, 1, 0);

        private float lastZoom = 0;
        private Vector3 lastCamPosition;

        float smoothTime = 0.3f;
        float yVelocity = 0.0f;
        float xVelocity = 0.0f;
        Vector2 vec2Velocity = Vector2.zero;
        float orthoVelocity = 0.0f;
        bool panHappened = false;

        public override void init()
        {
            base.init();
            usedCamera.orthographicSize = settingsCamera.orthoSize;
            zoom = usedCamera.orthographicSize;
            cameraPosition = settingsCamera.cameraInitPosition;
            usedCamera.transform.position = cameraPosition;
            cameraTransform = usedCamera.transform;
            targetCamSizeOnZoom = settingsCamera.targetCamSizeOnZoom;

            currentCameraOrthoTarget = zoom;
            currentCameraTarget = cameraTransform.position;

#if UNITY_EDITOR
            Cursor.visible = true;
#else
         Cursor.visible = false;

#endif
        }

        public void OnCameraZoom(InputAction.CallbackContext context)
        {

            float v = context.ReadValue<float>();
            scrolling = (int)v;
        }

        public void OnPanMouseStatus(InputAction.CallbackContext context)
        {
            if (isPanByMiddleMouseOn == false) return;
            //right mouse click
            float status = context.ReadValue<float>();
            isDraging = status == 1 ? true : false;
        }

        public void OnPan(InputAction.CallbackContext context)
        {
            // Debug.Log("Phase: " + context.phase);
            if (context.phase != InputActionPhase.Canceled)
            {
                Vector2 v = context.ReadValue<Vector2>();
                panOffset = v;
                panHappened = true;
            }
            else
            {
                panHappened = false;
                panOffset = Vector2.zero;
            }

        }

        public void OnMouseOffset(InputAction.CallbackContext context)
        {
            Vector2 v = context.ReadValue<Vector2>();
            mouseOffset = v;
        }

        public void moveCameraTo(Vector3 targetPos)
        {
            moveToControl.startMoveTo(transform, transform.position, targetPos, usedCamera, targetCamSizeOnZoom);
            cameraPosition = cameraTransform.position;
            currentCameraTarget = cameraPosition;
            zoom = usedCamera.orthographicSize;
        }



        public void Update()
        {
            float delta = Time.deltaTime;
            base.onUpdate(delta);
            if (settingsCamera.staticCam) return;

            //check for zoom
            if (settingsCamera.zoomOn && CommonCode.Common.IsPointerOverUIElement() == false)
            {
                updateZoom(delta);
            }

            if (moveToControl.isMoving())
            {
                moveToControl.updateMoveTo(delta);
                cameraPosition = cameraTransform.position;
                zoom = usedCamera.orthographicSize;
            }

            //check for pan
            if (settingsCamera.panOn && !moveToControl.isMoving() && isCameraPositionLocked == false)
            {
                updatePan(delta);
            }

            smoothMoveUpdate(delta);

            updateCamRect();
        }

        public Vector3 clampTargetOrtho(Vector3 cur, float target)
        {
            Vector3 curLast = cur;
            float current = usedCamera.orthographicSize;
            Vector3 offset = Vector3.zero;
            usedCamera.transform.position = cur;
            usedCamera.orthographicSize = target;
            //sprawdzamy czy wszystkie punkty sa w zakresie rozmiaru świata gry
            Vector3 bottomLeft = usedCamera.ViewportToWorldPoint(bottomLeftConst);
            if (!isInsideWorld(bottomLeft))
            {
                //compute offset
                offset = getOffsetFor(bottomLeft);
                offset -= bottomLeft;
                cameraTransform.position += offset;
            }

            Vector3 topLeft = usedCamera.ViewportToWorldPoint(topLeftConst);
            if (!isInsideWorld(topLeft))
            {
                //compute offset
                offset = getOffsetFor(topLeft);
                offset -= topLeft;
                cameraTransform.position += offset;
            }

            Vector3 topRight = usedCamera.ViewportToWorldPoint(topRightConst);
            if (!isInsideWorld(topRight))
            {
                //compute offset
                offset = getOffsetFor(topRight);
                offset -= topRight;
                cameraTransform.position += offset;
            }

            Vector3 bottomRight = usedCamera.ViewportToWorldPoint(bottomRightConst);
            if (!isInsideWorld(bottomRight))
            {
                //compute offset
                offset = getOffsetFor(bottomRight);
                offset -= bottomRight;
                cameraTransform.position += offset;
            }

            Vector3 res = cameraTransform.position;
            usedCamera.orthographicSize = current;
            cameraTransform.position = curLast;
            return res;
        }


        public Vector3 clampTarget(Vector3 cur, Vector3 target)
        {
            Vector3 curLast = cur;
            Vector3 offset = Vector3.zero;
            cameraTransform.position = target;
            //sprawdzamy czy wszystkie punkty sa w zakresie rozmiaru świata gry
            Vector3 bottomLeft = usedCamera.ViewportToWorldPoint(bottomLeftConst);
            if (!isInsideWorld(bottomLeft))
            {
                //compute offset
                offset = getOffsetFor(bottomLeft);
                offset -= bottomLeft;
                cameraTransform.position += offset;
            }

            Vector3 topLeft = usedCamera.ViewportToWorldPoint(topLeftConst);
            if (!isInsideWorld(topLeft))
            {
                //compute offset
                offset = getOffsetFor(topLeft);
                offset -= topLeft;
                cameraTransform.position += offset;
            }

            Vector3 topRight = usedCamera.ViewportToWorldPoint(topRightConst);
            if (!isInsideWorld(topRight))
            {
                //compute offset
                offset = getOffsetFor(topRight);
                offset -= topRight;
                cameraTransform.position += offset;
            }

            Vector3 bottomRight = usedCamera.ViewportToWorldPoint(bottomRightConst);
            if (!isInsideWorld(bottomRight))
            {
                //compute offset
                offset = getOffsetFor(bottomRight);
                offset -= bottomRight;
                cameraTransform.position += offset;
            }

            Vector3 res = cameraTransform.position;
            cameraTransform.position = curLast;
            return res;
        }


        public void smoothMoveUpdate(float delta)
        {
            //sprawdzmy czy target jest w zakresie wielkkosci mapy

            Vector3 current = cameraTransform.transform.position;
            Vector3 targetPos = currentCameraTarget;

            currentCameraTarget = clampTarget(current, targetPos);

            //clam ortho

            //  currentCameraTarget = clampTargetOrtho(currentCameraTarget, targetCamSizeOnZoom);

            currentCameraTarget.z = -10;
            float smoothTimeLocal = 0.1f;
            if (isDraging)
            {
                smoothTimeLocal = settingsCamera.mouseDragSpeed;
            }
            else
            {
                smoothTimeLocal = settingsCamera.panSpeed;
            }

            /////////////////////////////////////////////////////
            ///

            //smooth movement
            Vector2 from = new Vector2(transform.position.x, transform.position.y);
            Vector2 result = Vector2.SmoothDamp(from, currentCameraTarget, ref vec2Velocity, smoothTimeLocal);

            /*
                float newPositionX = Mathf.SmoothDamp(transform.position.x, currentCameraTarget.x, ref xVelocity, smoothTimeLocal);
                float newPositionY = Mathf.SmoothDamp(transform.position.y, currentCameraTarget.y, ref yVelocity, smoothTimeLocal);

            newPositionX = currentCameraTarget.x;
            newPositionY = currentCameraTarget.y;*/

            float newPositionX = result.x;
            float newPositionY = result.y;

            cameraTransform.transform.position = new Vector3(newPositionX, newPositionY, -10);

            //smooth ortho
            float newOrtho = Mathf.SmoothDamp(usedCamera.orthographicSize, currentCameraOrthoTarget, ref orthoVelocity, smoothTimeLocal);
            usedCamera.orthographicSize = newOrtho;

        }


        public virtual void updatePan(float delta)
        {

            lastCamPosition = cameraPosition;
            Vector3 currentOffset = Vector3.zero;
            if (isDraging)
            {
                //  cameraPosition -= mouseOffset * delta * settingsCamera.mouseDragSpeed;
                currentOffset -= mouseOffset /* delta * settingsCamera.mouseDragSpeed*/;
                mouseOffset = Vector2.zero;
                currentCameraTarget = cameraTransform.position + currentOffset;

            }
            else
            {

                if (isPanByMiddleMouseOn == false && panHappened == false)
                {
                    //sprawdzamy pozycje kamery
                    Vector2 posMouse = Mouse.current.position.ReadValue();
                    //konwertujemy pozycje to viewportu
                    Vector2 viewportPos = usedCamera.ScreenToViewportPoint(posMouse);

                    //sprawdzamy czy pozycja myszki nie jest na krancu którejś cześci ekranu
                    //lewa czesc i prawa
                    if (viewportPos.x < panScreenEdgePercentage)
                    {
                        panOffset.x = -1;
                    }
                    else if (viewportPos.x > 1.0f - panScreenEdgePercentage)
                    {
                        panOffset.x = 1;
                    }
                    else panOffset.x = 0;

                    //góra / dół
                    if (viewportPos.y > 1.0f - panScreenEdgePercentage)
                    {
                        panOffset.y = 1.0f;
                    }
                    else if (viewportPos.y < panScreenEdgePercentage)
                    {
                        panOffset.y = -1.0f;
                    }
                    else panOffset.y = 0;
                }

                currentOffset += new Vector3(panOffset.x /* settingsCamera.horizontalPanSpeed * delta*/, panOffset.y/* * settingsCamera.verticalPanSpeed * delta*/, 0);
                currentCameraTarget = cameraTransform.position + currentOffset;

            }



            //  cameraTransform.position = cameraPosition;
        }



        public virtual void updateZoom(float delta)
        {

            lastZoom = zoom;
            if (scrolling < 0)
            {
                currentCameraOrthoTarget += settingsCamera.zoomInSpeed;
            }

            if (scrolling > 0)
            {

                currentCameraOrthoTarget -= settingsCamera.zoomOutSpeed;
            }

            currentCameraOrthoTarget = Mathf.Clamp(currentCameraOrthoTarget, settingsCamera.minZoomOffset, settingsCamera.maxZoomOffset);
            // usedCamera.orthographicSize = zoom;
        }

        Vector2 getOffsetFor(Vector2 pos)
        {
            Vector2 moveOffset = pos;
            //x
            if (pos.x > settingsCamera.gameMapRect.x + settingsCamera.gameMapRect.width)
            {
                moveOffset.x = (settingsCamera.gameMapRect.x + settingsCamera.gameMapRect.width);
            }
            else if (pos.x < settingsCamera.gameMapRect.x)
            {
                moveOffset.x = (settingsCamera.gameMapRect.x);
            }
            //y
            if (pos.y > settingsCamera.gameMapRect.y + settingsCamera.gameMapRect.height)
            {
                moveOffset.y = (settingsCamera.gameMapRect.y + settingsCamera.gameMapRect.height);
            }
            else if (pos.y < settingsCamera.gameMapRect.y)
            {
                moveOffset.y = (settingsCamera.gameMapRect.y);
            }
            return moveOffset;
        }


        //zablokuj kamere tak zeby nie wyszła poza widok mapy
        void updateCamRect()
        {
            return;
            //now check pan
        /*    cameraTransform.position = cameraPosition;
            bool canDo = true;
            //sprawdzamy czy wszystkie punkty sa w zakresie rozmiaru świata gry
            Vector2 bottomLeft = usedCamera.ViewportToWorldPoint(bottomLeftConst);
            if (!isInsideWorld(bottomLeft))
            {
                canDo = false;
            }

            Vector2 topLeft = usedCamera.ViewportToWorldPoint(topLeftConst);
            if (!isInsideWorld(topLeft))
            {
                canDo = false;
            }

            Vector2 topRight = usedCamera.ViewportToWorldPoint(topRightConst);
            if (!isInsideWorld(topRight))
            {
                canDo = false;
            }

            Vector2 bottomRight = usedCamera.ViewportToWorldPoint(bottomRightConst);
            if (!isInsideWorld(bottomRight))
            {
                canDo = false;
            }

            if (canDo == false)
            {
                //wracamy do poprzedniej wersji
                cameraPosition = lastCamPosition;
                cameraTransform.position = cameraPosition;
            }


            //    usedCamera.orthographicSize = zoom;
            //test camera
            Vector3 offset;
            //sprawdzamy czy wszystkie punkty sa w zakresie rozmiaru świata gry
            bottomLeft = usedCamera.ViewportToWorldPoint(bottomLeftConst);
            if (!isInsideWorld(bottomLeft))
            {
                //compute offset
                offset = getOffsetFor(bottomLeft);
                cameraTransform.position += offset;
            }

            topLeft = usedCamera.ViewportToWorldPoint(topLeftConst);
            if (!isInsideWorld(topLeft))
            {
                //compute offset
                offset = getOffsetFor(topLeft);
                cameraTransform.position += offset;
            }

            topRight = usedCamera.ViewportToWorldPoint(topRightConst);
            if (!isInsideWorld(topRight))
            {
                //compute offset
                offset = getOffsetFor(topRight);
                cameraTransform.position += offset;
            }

            bottomRight = usedCamera.ViewportToWorldPoint(bottomRightConst);
            if (!isInsideWorld(bottomRight))
            {
                //compute offset
                offset = getOffsetFor(bottomRight);
                cameraTransform.position += offset;
            }
*/
            //    cameraPosition = cameraTransform.position;
        }

        bool isInsideWorld(Vector2 p)
        {
            return settingsCamera.gameMapRect.Contains(p);
        }
    }
}



