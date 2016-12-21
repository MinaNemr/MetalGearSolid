using System;
using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

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
		GameObject itemClicked = null;
		GameObject itemDeselected = null;
		private AudioSource audio;
		private ThirdPersonCharacter enemy;
		private bool heard = false;
		private Vector3 dest;
		private Vector3 move;
		private bool first = false;
		private bool reached1 = false;
		private bool reached2 = false;
		private bool reached3 = false;
		public GameObject M9;
		public GameObject AK47;
		public GameObject patriot;
		public GameObject key;
		public GameObject ration;
		public GameObject cigarette;
	    private List<string> items = new List<string>();
		private List<string> weapons = new List<string>();
		 Canvas itemsMenu;
		Canvas weaponsMenu;
		public Button button;
		int gapW=250;
		int gapI=250;
		int health = 100;
		public Text health_t;
        private void Start()
        {
			weaponsMenu=GameObject.Find("weaponsMenu").GetComponent<Canvas>();
			itemsMenu=GameObject.Find("itemsMenu").GetComponent<Canvas>();
			itemsMenu.enabled = false;
			weaponsMenu.enabled = false;
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
			if(Input.GetKeyDown(KeyCode.RightAlt) && itemClicked != null && itemDeselected==null){
				Debug.Log ("alt");
				itemClicked.SetActive (true);
				itemDeselected = itemClicked;
				itemClicked = null;
			}
			else if(Input.GetKeyDown(KeyCode.RightAlt) && itemClicked == null && itemDeselected!=null){
				itemDeselected.SetActive (false);
				itemDeselected = null;
			}

			health_t.text = "Snake Health: " + health;
			if (Input.GetKeyDown ("r")) {
				Debug.Log(itemsMenu.enabled);
				itemsMenu.enabled = !itemsMenu.enabled;
			}

			else if(Input.GetKeyDown("f")){
				weaponsMenu.enabled = !weaponsMenu.enabled; 
			}

            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

			if (Input.GetKeyDown ("l") || heard) {
				if (Input.GetKeyDown ("l")) {
					audio.Play ();
					m_Character.getAnimator().SetBool("cough", true);
				/*if (m_Character.getAnimator().GetCurrentAnimatorStateInfo (0).IsName ("cough")) {
						m_Character.getAnimator().SetBool ("cough", false);
					}*/
				}


				if (enemy == null) {

					Collider[] possibleEnemiesWhoHeardMe = Physics.OverlapSphere (transform.position, 20);
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

		public void setHealth(int h){
			health = h;
		}

		void M9OnClick(){
			itemClicked = M9;
		}

		void AK47OnClick(){
			itemClicked = AK47;
		}
			
		void patriotOnClick(){
			itemClicked = patriot;

		}


		void keyOnClick(){
			itemClicked = key;

		}

		void rationOnClick(){
			itemClicked = ration;

		}

		void cigaretteOnClick(){
			itemClicked = cigarette;

		}



		void OnCollisionEnter(Collision other)
		{
			if (other.transform.tag == "bullet") {
				Destroy (other.gameObject);
				health -= 20;
			}

			if (other.transform.tag == "M9" || other.transform.tag == "AK47" || other.transform.tag == "patriot") {
				weapons.Add (other.transform.tag);
				Button newButton = (Button)Instantiate(button);  
				newButton.transform.SetParent(weaponsMenu.transform,false);
				newButton.GetComponentInChildren<Text>().text = other.transform.tag;
				newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400, gapI);
				if (other.transform.tag == "M9") {
					newButton.onClick.AddListener (M9OnClick);
				}
				else if (other.transform.tag == "AK47") {
					newButton.onClick.AddListener (AK47OnClick);
				}
				else if (other.transform.tag == "patriot") {
					newButton.onClick.AddListener (patriotOnClick);
				}

				gapI -= 130;
				Debug.Log (other.transform.tag);
				other.gameObject.SetActive (false);
			} else if (other.transform.tag == "key" || other.transform.tag == "ration" || other.transform.tag == "cigarette"){
				if (!items.Contains(other.transform.tag)) {
					items.Add (other.transform.tag);
					Button newButton = (Button)Instantiate(button);  
					newButton.transform.SetParent(itemsMenu.transform,false);
					newButton.GetComponentInChildren<Text>().text = other.transform.tag;
					newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400, gapW);
					if (other.transform.tag == "key") {
						newButton.onClick.AddListener (keyOnClick);
					}
					else if (other.transform.tag == "ration") {
						newButton.onClick.AddListener (rationOnClick);
					}
					else if (other.transform.tag == "cigarette") {
						newButton.onClick.AddListener (cigaretteOnClick);
					}
					gapW -= 130;
					Debug.Log (other.transform.tag);
					other.gameObject.SetActive (false);
				}

			}
		}
    }
}
