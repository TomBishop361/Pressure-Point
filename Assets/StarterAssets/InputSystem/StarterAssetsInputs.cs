using System.Collections;
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

        public void OnMove(InputValue value)
		{
            MoveInput(value.Get<Vector2>());
			Debug.Log(move);
		}

		public void OnInteract()
        {
			if (playerState == 0)
			{
				RaycastHit hit;

				Vector3 p1 = Camera.main.transform.position;
				float distanceToObstacle = 0;

				// Cast a sphere wrapping character controller 10 meters forward
				// to see if it is about to hit anything.
				if (Physics.SphereCast(p1, 0.08f, Camera.main.transform.forward, out hit, 2.0f))
				{
					distanceToObstacle = hit.distance;
					Debug.Log(distanceToObstacle);
					if (hit.transform.CompareTag("Valve"))
					{
						Debug.Log("Valve HIT");
						InteractedObj = hit.transform.gameObject;
						playerState = 1;//valve state
						StartCoroutine(LerpToInteract());
					}
				}
			}
			else
			{
				playerState = 0;
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

        public static float lerp(float startValue, float endValue, float t)
        {
            return (startValue + (endValue - startValue) * t);
        }

		IEnumerator LerpToInteract()
		{
			lerping = true;
			float time = 0;
			while (time < 1f)
			{
				float perc = 0;
				perc = Easing.Linear(time);
				lerpX = lerp(transform.position.x, InteractedObj.GetComponent<ValveScript>().interactPos.x,perc);
				lerpZ = lerp(transform.position.z, InteractedObj.GetComponent<ValveScript>().interactPos.y, perc);
                lerpRotationy = lerp(transform.rotation.y, _lookRotation.y, perc);
                time += Time.deltaTime;
				yield return null;
			}
			lerping= false;
		}


        private void Update()
        {
			if (InteractedObj != null)
			{
				_direction = (InteractedObj.transform.position - transform.position).normalized;

				//create the rotation we need to be in to look at the target
				_lookRotation = Quaternion.LookRotation(_direction);
			}

			if (lerping) { 
				//moves player to correct position to interact
				transform.eulerAngles = new Vector3(transform.rotation.y, lerpRotationy, transform.rotation.z); //transforms the rotation of the camera        
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
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}