using System;
using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

		private AudioSource audio;
		private ThirdPersonCharacter enemy;
		private bool heard = false;
		private Vector3 dest;
		private Vector3 move;
		private bool first = false;
		private bool reached1 = false;
		private bool reached2 = false;
		private bool reached3 = false;


        private void Start()
        {
			audio = GetComponent<AudioSource> ();
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

			if (Input.GetKeyDown ("l") || heard) {
				if (Input.GetKeyDown ("l")) {
					audio.Play ();
				}


				if (enemy == null) {

					Collider[] possibleEnemiesWhoHeardMe = Physics.OverlapSphere (transform.position, 10);
					List <float> distances = new List<float> ();
					List <Collider> enemies = new List<Collider> ();
					float distanceToPlayer;

					foreach (Collider hit in possibleEnemiesWhoHeardMe) {
						if (hit.transform.tag == "enemy") {
							distanceToPlayer = Vector3.Distance (hit.transform.position, transform.position);
							distances.Add (distanceToPlayer);
							enemies.Add (hit);
						}
					}

					distances.Sort ();
					Debug.Log (distances);
					if (distances.Count != 0) {
					
						int index = distances.IndexOf (distances [0]);
						Debug.Log (index);
						enemy = enemies [index].GetComponent<ThirdPersonCharacter> ();
						//enemy.setConstraints(RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ);
						Debug.Log ("heard");
						enemy.Stop (true);
						heard = true;
						dest = enemy.transform.position;
						move = (transform.position - enemy.transform.position);
						enemy.Move (move, false, false);
					}

				} else {
					enemy.Stop (true);
					heard = true;
					dest = enemy.transform.position;
					move = (transform.position - enemy.transform.position);
					enemy.Move (move, false, false);
				}
			}
			if (enemy) {
				if (!reached1) {
					if (Mathf.Abs (dest.z - transform.position.z) < 0.5) {

						heard = false;
						move = Vector3.zero;
						enemy.Move (move, false, false);
						reached1 = true;
					} 
				} else if (!reached2) {
					move = Vector3.zero;
					enemy.Move (move, false, false);
					reached2 = true;
					Invoke ("backToPattern", 2);

				} else {
					if (first) {
						if (!reached3)
							backToPattern ();
					} else {
						move = Vector3.zero;
						enemy.Move (move, false, false);
					}
				}
			}

			if (!reached3 && enemy) {
				if (Mathf.Abs (enemy.getCurrent ().z - enemy.transform.position.z) < 0.05) {
					reached3 = false;
					first = false;
					reached2 = false;
					reached1 = false;
					enemy.Stop (false);
					enemy.setConstraints ( RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ);

					enemy = null;
				}
			} 
        }

		private void backToPattern(){
			move = enemy.getCurrent () - enemy.transform.position;
			enemy.Move (move, false, false);
			first = true;
		}

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}
