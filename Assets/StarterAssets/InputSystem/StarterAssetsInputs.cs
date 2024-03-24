using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements.Experimental;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[SerializeField] GameObject cam;

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Misc")]
		public int playerState =0;
		public GameObject InteractedObj;
		Vector3 _direction;
        private Quaternion _lookRotation;
        float lerpRotationy;
		float lerpX;
		float lerpZ;
		bool lerping;
		GameObject HeldObject;
		bool isHeldObject;

        public void OnMove(InputValue value)
		{
            MoveInput(value.Get<Vector2>());

		}

		public void OnInteract()
        {
			if (playerState == 0)
			{
				RaycastHit hit;

				Vector3 p1 = Camera.main.transform.position;

				// Cast a sphere wrapping character controller 10 meters forward
				// to see if it is about to hit anything.
				if (Physics.SphereCast(p1, 0.08f, Camera.main.transform.forward, out hit, 2.0f))
				{
					
					Debug.Log(hit.transform.name);
					if(hit.transform != null)
					{
						Debug.Log("hit");
					}
					if (hit.transform.CompareTag("PC"))
					{
						Debug.Log("PC Hit");
						InteractedObj = hit.transform.gameObject;
						playerState = 2; //PC State
                        StartCoroutine(LerpToInteract());
                    }
					if (hit.transform.CompareTag("Valve"))
					{
						Debug.Log("Valve HIT");
						InteractedObj = hit.transform.gameObject;
						playerState = 1;//valve state
						StartCoroutine(LerpToInteract());
					}
					if (hit.transform.CompareTag("PickUp"))
					{
						Debug.Log("Hit");
						if (isHeldObject == false)
						{
							isHeldObject = true;
							hit.transform.SetParent(Camera.main.transform, false);
							hit.transform.localPosition = new Vector3(0.435f, -0.5f, 0.741f);
							hit.transform.transform.localEulerAngles = new Vector3(0, 0, 0);
							HeldObject = hit.transform.gameObject;
						}
					}
					if (hit.transform.CompareTag("Screw")) {
						if (HeldObject != null && HeldObject.transform.name == "Screwdriver")
						{
							hit.transform.GetComponent<Screw>().UnScrew();
						}
					}
					if (hit.transform.CompareTag("Switch"))
					{
						hit.transform.GetComponent<Switch>().flip();
					}
                } 
			}
			else if(playerState == 1) 
			{
				playerState = 0;
			}
		}

        void OnDrop()
        {
            if (isHeldObject == true)
            {
                HeldObject.transform.parent = null;
				if(HeldObject.transform.name == "Screwdriver")
				{
                    HeldObject.transform.position = new Vector3(-24.311f, 9.123f, -24.76f);
                    HeldObject.transform.localEulerAngles = new Vector3(90, 90, 0);
				}
				else
				{
                    HeldObject.transform.position = new Vector3(-20.306f, 7.5f, -27.129f);
                    HeldObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                } 
                isHeldObject = false;
            }
        }


        public void OnLook(InputValue value)
		{
			switch (playerState)
			{
				case 0:
					if (cursorInputForLook)
					{
						LookInput(value.Get<Vector2>());
					}
					break;
				case 1: // Valve turning
					look = new Vector2 (0,0);
					
					break;
				case 2://PC
                    if (cursorInputForLook)
                    {
                        LookInput(value.Get<Vector2>());
                    }
                    break;

            }
			
		}

		public void OnJump(InputValue value)
		{
			switch (playerState)
			{
				case 0:
					JumpInput(value.isPressed);
					break;
				case 1: // Valve turning

					break;

			}
			
		}

		public void OnSprint(InputValue value)
		{
			switch (playerState)
			{
				case 0:
					SprintInput(value.isPressed);
					break;
				case 1: // Valve turning

					break;

			}
			
		}

        

		//lerp subrotine 
		IEnumerator LerpToInteract()
		{
			lerping = true;
			float time = 0;
			while (time < 1f)
			{
				float perc = 0;
				perc = Easing.Linear(time);
				lerpX = LerpScript.lerp(transform.position.x, InteractedObj.GetComponent<PosData>().interactPos.x,perc);
				lerpZ = LerpScript.lerp(transform.position.z, InteractedObj.GetComponent<PosData>().interactPos.y, perc);
                time += Time.deltaTime;
				yield return null;
			}
			lerping= false;			
		}


        private void Update()
        {
			//calculates roation of interacted object
			if (InteractedObj != null)
			{				
                _direction = (InteractedObj.transform.position - transform.position);
                _lookRotation = Quaternion.LookRotation(_direction);
            }

			if (lerping) {
                //moves player to correct position to interact
                _lookRotation.x = transform.rotation.x; _lookRotation.z = transform.rotation.z; //Changes values so Y is the only axis that rotates
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 20);  
                transform.position = new Vector3(lerpX, transform.position.y, lerpZ);
			}

        }



        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			//Change so this only happens when player is NOT in terminal state
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}