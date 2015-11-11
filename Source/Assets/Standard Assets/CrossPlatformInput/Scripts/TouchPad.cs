using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	[RequireComponent(typeof(Image))]
	public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// Options for which axes to use
		public enum AxisOption
		{
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}


		public enum ControlStyle
		{
			Absolute, // operates from teh center of the image
			Relative, // operates from the center of the initial touch
			Swipe, // swipe to touch touch no maintained center
		}


		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public ControlStyle controlStyle = ControlStyle.Absolute; // control style to use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
		public float Xsensitivity = 1f;
		public float Ysensitivity = 1f;

		Vector3 m_StartPos;
		Vector2 m_PreviousDelta;
		Vector3 m_JoytickOutput;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input
		bool m_Dragging;
		int m_Id = -2;
		Vector2 m_PreviousTouchPos; // swipe style control touch

        //Shane added these images
        public Image m_ThumbBackground;
        public Image m_ThumbForeground;


#if !UNITY_EDITOR
    private Vector3 m_Center;
    private Image m_Image;
#else
		Vector3 m_PreviousMouse;
#endif

		void OnEnable()
		{
			CreateVirtualAxes();
            //Shane added this to hide the images until touch
            m_ThumbBackground.enabled = false;
            m_ThumbForeground.enabled = false;
		}

        void Start()
        {
#if !UNITY_EDITOR
            m_Image = GetComponent<Image>();
            m_Center = m_Image.transform.position;
#endif
        }

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
			}
		}

		void UpdateVirtualAxes(Vector3 value)
		{
            //value = value.normalized; Shane removed this so we had analog input, not binary (on/off)
            if (m_UseX)
            {
                m_HorizontalVirtualAxis.Update(value.x);
            }
            if (m_UseY)
            {
               m_VerticalVirtualAxis.Update(value.y);
            }
		}


		public void OnPointerDown(PointerEventData data)
		{
            if(controlStyle != ControlStyle.Swipe)
            {
                m_ThumbBackground.enabled = true;
                m_ThumbForeground.enabled = true;
                m_ThumbBackground.rectTransform.position = data.position;
            }
            else
            {
                m_PreviousTouchPos = data.position;
            }
			m_Dragging = true;
			m_Id = data.pointerId;
#if !UNITY_EDITOR
        if (controlStyle != ControlStyle.Absolute )
            m_Center = data.position;
#endif
		}

		void Update()
		{
            
            if (!m_Dragging)
            {
                return;
            }
            //Shane added this "dropped" variable for a special case:
            //If the user put touch[0] on the right side of the screen, then touch[1] on the left
            //and then removed touch[0] from the right side - then the left touch became touch[0]
            //as we update this based on touchID, we needed to make sure that it could handle this
            //we set dropped to false at the start of Update() because if we then put a finger on the right side of the screen, touch[0] becomes touch[1] again
            bool dropped = false;

            if (Input.touchCount == m_Id && m_Id != -2)
            {
                //Drop touch ID in case of other touch being released
                m_Id -= 1;
                dropped = true;
            }

            if (Input.touchCount >= m_Id + 1 && m_Id != -2)
            {

#if !UNITY_EDITOR

                if(controlStyle == ControlStyle.Swipe)
                {
                    Vector2 pointerDelta;
                    pointerDelta.x = Input.touches[m_Id].position.x - m_PreviousTouchPos.x;
                    pointerDelta.y = Input.touches[m_Id].position.y - m_PreviousTouchPos.y;
                    pointerDelta = Vector2.ClampMagnitude(pointerDelta, 100);
                    m_PreviousTouchPos = Input.touches[m_Id].position;
                    UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
                }
                else
                {
                    Vector2 pointerDelta;
                    pointerDelta.x = Input.touches[m_Id].position.x - m_ThumbBackground.rectTransform.position.x;
                    pointerDelta.y = Input.touches[m_Id].position.y - m_ThumbBackground.rectTransform.position.y;
                    pointerDelta = Vector2.ClampMagnitude(pointerDelta, 100);

                    m_ThumbForeground.rectTransform.localPosition = pointerDelta;
                    UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
                }
			}

#else
            }
            //Shane completely rewrote this to suit the style he wanted
            if (controlStyle == ControlStyle.Swipe)
            {
                Vector2 pointerDelta;
                pointerDelta.x = Input.mousePosition.x - m_PreviousTouchPos.x;
                pointerDelta.y = Input.mousePosition.y - m_PreviousTouchPos.y;
                pointerDelta = Vector2.ClampMagnitude(pointerDelta, 100);
                m_PreviousTouchPos = Input.mousePosition;
                UpdateVirtualAxes(
                    new Vector3(
                        pointerDelta.x * (Mathf.Abs(pointerDelta.x * 0.25f)), 
                        pointerDelta.y * (Mathf.Abs(pointerDelta.y * 0.25f)), 
                        0)
                    );
            }
            else
            {
                Vector2 pointerDelta;
                pointerDelta.x = Input.mousePosition.x - m_ThumbBackground.rectTransform.position.x;
                pointerDelta.y = Input.mousePosition.y - m_ThumbBackground.rectTransform.position.y;
                if (Mathf.Abs(pointerDelta.x) < 10) pointerDelta.x = 0;
                if (Mathf.Abs(pointerDelta.y) < 10) pointerDelta.y = 0;

                pointerDelta = Vector2.ClampMagnitude(pointerDelta, 100);
                m_ThumbForeground.rectTransform.localPosition = pointerDelta;
                UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
            }
#endif
                
            if(dropped)
            {
                //if we dropped, return in case old touch is replaced
                m_Id += 1;
            }
            

		}


		public void OnPointerUp(PointerEventData data)
		{
			m_Dragging = false;
			m_Id = -2;
			UpdateVirtualAxes(Vector3.zero);
            m_ThumbBackground.enabled = false;
            m_ThumbForeground.enabled = false;
		}

		void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);

			if (CrossPlatformInputManager.AxisExists(verticalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
		}
	}
}