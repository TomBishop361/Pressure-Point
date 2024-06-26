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
		public int playerState = 0;
		public GameObject InteractedObj;
		Vector3 _direction;
		private Quaternion _lookRotation;
		float lerpRotationy;
		float lerpX;
		float lerpZ;
		bool lerping;
		GameObject HeldObject;
		bool isHeldObject;
		public bool isRepairing;
		float interacting;
		[HideInInspector]public bool BlowTourchFire = false;

        public void OnMove(InputValue value){ 
			move = value.Get<Vector2>();
		}

		public void setPlayerStateZero() //sets player back to state 0 (called by Terminal button)
		{
			playerState = 0;
		}		

		public void OnInteract(InputValue value)
		{
			interacting = value.Get<float>();
			if (playerState == 0)
			{
				if (interacting == 1)//if mouse down
				{
					if(HeldObject != null && HeldObject.name == "BlowTorch")
					{
						BlowTourchFire = true;
					}
					RaycastHit hit;

					Vector3 p1 = Camera.main.transform.position;

					// Cast a sphere wrapping character controller 10 meters forward
					// to see if it is about to hit anything.
					if (Physics.SphereCast(p1, 0.08f, Camera.main.transform.forward, out hit, 2.0f))
					{
						
						//Comparing hit object tag to check what action to perform					
						if (hit.transform.CompareTag("PC"))
						{
							
							hit.transform.GetComponent<BoxCollider>().enabled = false;
							InteractedObj = hit.transform.gameObject;
							playerState = 2; //PC State
							StartCoroutine(LerpToInteract());
							hit.transform.GetComponent<TerminalManager>().inputField.enabled = true;
							hit.transform.GetComponent<TerminalManager>().inputField.ActivateInputField();

						}
						if (hit.transform.CompareTag("Valve"))
						{
							
							InteractedObj = hit.transform.gameObject;
							playerState = 1;//valve state
							StartCoroutine(LerpToInteract());
							return;
						}
						if (hit.transform.CompareTag("PickUp"))
						{
							if (isHeldObject == false)
							{
								isHeldObject = true;
								hit.transform.SetParent(Camera.main.transform, false);
								hit.transform.localPosition = new Vector3(0.435f, -0.5f, 0.741f);
								hit.transform.transform.localEulerAngles = new Vector3(0, 0, 0);
								HeldObject = hit.transform.gameObject;
							}
						}
						if (hit.transform.CompareTag("Screw"))
						{
							if (HeldObject != null && HeldObject.transform.name == "Screwdriver")
							{
								hit.transform.GetComponent<Screw>().UnScrew();
							}
						}
						if (hit.transform.CompareTag("Switch"))
						{
							hit.transform.GetComponent<Switch>().flip();
						}
						if (hit.transform.CompareTag("Breach"))
						{
							if (HeldObject != null && HeldObject.transform.name == "BlowTorch")
							{
								isRepairing = true;

							}
						}
						else {
							BlowTourchFire = false;
							isRepairing = false;
						}

					}
				}

			}
			if(interacting == 0)
			{
				BlowTourchFire = false;
			}
			
            if (playerState == 1)
            {
				if (interacting == 1)
				{
					playerState = 0;
					return;
				}

            }
        }

		//Drops held item
		void OnDrop()
		{
			if (isHeldObject == true)
			{
				HeldObject.transform.parent = null;
				if (HeldObject.transform.name == "Screwdriver")
				{
					HeldObject.transform.position = new Vector3(-24.311f, 9.123f, -24.76f);
					HeldObject.transform.localEulerAngles = new Vector3(90, 90, 0);
				}
				else
				{
					HeldObject.transform.position = new Vector3(-20.186f, 8.375f, -27.338f);
					HeldObject.transform.localEulerAngles = new Vector3(0, 0, 0);
				}
				isHeldObject = false;
				HeldObject = null;
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
					look = new Vector2(0, 0);

					break;
				case 2://PC
					if (cursorInputForLook)
					{
						LookInput(value.Get<Vector2>());
					}
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
			}
		}



		//lerp subrotine 
		IEnumerator LerpToInteract()
		{
			lerping = true;
			float time = 0;
			while (time < 0.25f)
			{
				float perc = 0;
				perc = Easing.Linear(time);
				lerpX = LerpScript.lerp(transform.position.x, InteractedObj.GetComponent<PosData>().interactPos.x, perc);
				lerpZ = LerpScript.lerp(transform.position.z, InteractedObj.GetComponent<PosData>().interactPos.y, perc);
				time += Time.deltaTime;
				yield return null;
			}
			lerping = false;
		}


		private void Update()
		{
			
			//calculates roation of interacted object
			if (InteractedObj != null)
			{
				_direction = (InteractedObj.transform.position - transform.position);
				_lookRotation = Quaternion.LookRotation(_direction);
			}

			if (lerping)
			{
				//moves player to correct position to interact
				_lookRotation.x = transform.rotation.x; _lookRotation.z = transform.rotation.z; //Changes values so Y is the only axis that rotates
				transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, 20);
				transform.position = new Vector3(lerpX, transform.position.y, lerpZ);
			}

			

		}

        private void FixedUpdate()
        {     
            if(interacting == 0 )isRepairing = false;

        }



        public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
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