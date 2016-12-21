using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class Soldier1UserControl : MonoBehaviour
    {
		public bool playerInRange;

		[SerializeField]
		Transform lineOfSightEnd;
		public CapsuleCollider snake_c;
		//shooting
		Transform player; //common
		public float range = 50.0f;
		public float bulletImpulse= 20.0f;
		private bool onRange= false;
		public Rigidbody projectile;
		public Canvas gameover;

		public ThirdPersonCharacter snake;
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;   
		private float end_p;
		public bool move = true;
		private bool direction = true;
		private bool first = true;
		private float start_p;
		private void Start ()
        {
			gameover.enabled = false;
            m_Character = GetComponent<ThirdPersonCharacter>();
			start_p = m_Character.transform.position.x;
			end_p = m_Character.transform.position.x + 40;
			playerInRange = false;
			player = GameObject.Find("Snake").transform;
			lineOfSightEnd =GameObject.Find("view").transform;
			m_Character.setCurrent(m_Character.transform.position);

			//shooting
		/*	float rand = UnityEngine.Random.Range(1.0f, 2.0f);
			InvokeRepeating("Shoot", 2, rand);*/

        }



        private void FixedUpdate()
        {
			if (!m_Character.isStop ()) {
				start_p = m_Character.transform.position.x;

				if (direction && start_p >= end_p) {
					direction = !direction;
					start_p = m_Character.transform.position.x;
					end_p = m_Character.transform.position.x - 40;
					move = false;
					first = false;
				} else if (!direction && start_p <= end_p) {
					direction = !direction;
					start_p = m_Character.transform.position.x;
					end_p = m_Character.transform.position.x + 40;
					move = false;
				}
				if (first) {
					m_Move = new Vector3(1,0,0)*0.5f;
					m_Character.Move (m_Move, false, false);
				} else if (move) {
					if (direction) {
						m_Move = new Vector3(1,0,0)*0.8f;
						m_Character.Move (m_Move, false, false);
					} else {
						m_Move = -new Vector3(1,0,0)*0.8f;
						m_Character.Move (m_Move, false, false);
					}
				} 
				else {
					m_Move = Vector3.zero;
					m_Character.Move(m_Move, false, false);
					Invoke ("change", 2);
				}

				if (CanPlayerBeSeen ()) {
					Debug.Log ("Seen");

					snake.getAnimator ().SetBool ("dead", true);
					Invoke ("die", 2);
					Invoke ("gameOver", 4);
				}


			
			}/* else {
				m_Move = Vector3.zero;
				m_Character.Move (m_Move, false, false);
			}*/
        }
		public void die(){
			snake_c.height = 1;
		}
		public void gameOver(){
			gameover.enabled = true;
		}

		public void change(){
			if (direction) {
				m_Move = new Vector3(1,0,0)*0.8f;
				m_Character.Move (m_Move, false, false);
			} else {
				m_Move = -new Vector3(1,0,0)*0.8f;
				m_Character.Move (m_Move, false, false);
			}
		}





			
		void Update(){
			if (!m_Character.isStop ()) {
		m_Character.setConstraints (RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ);
		}
		}




		bool CanPlayerBeSeen()
		{
			
			if (playerInRange)
			{
				if (PlayerInFieldOfView())
					return (!PlayerHiddenByObstacles());
				else
					return false;

			}
			else
			{
				return false;
			}



		}
		void OnTriggerStay(Collider other)
		{
			if (other.transform.tag == "snake") {
				playerInRange = true;
				Debug.Log (playerInRange);
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.transform.tag == "snake")
				playerInRange = false;
		}

		bool PlayerInFieldOfView()
		{
			
			Vector3 directionToPlayer = player.position - transform.position; 
			Debug.DrawLine(transform.position, player.position, Color.magenta); 

			Vector3 lineOfSight = lineOfSightEnd.position - transform.position; 
			Debug.DrawLine(transform.position, lineOfSightEnd.position, Color.yellow); 


			float angle = Vector3.Angle(directionToPlayer, lineOfSight);


			if (angle < 65) {
				return true;
			}
			else {
				return false;
			}


		}

		bool PlayerHiddenByObstacles()
		{

			float distanceToPlayer = Vector3.Distance(transform.position, player.position);
			RaycastHit[] hits = Physics.RaycastAll(new Vector3(transform.position.x,transform.position.y+3.2f,transform.position.z), new Vector3(player.position.x,player.position.y+3.2f,player.position.z) - new Vector3(transform.position.x,transform.position.y+3.2f,transform.position.z), distanceToPlayer);
			Debug.DrawRay(new Vector3(transform.position.x,transform.position.y+3.3f,transform.position.z), new Vector3(player.position.x,player.position.y+3.2f,player.position.z)  - new Vector3(transform.position.x,transform.position.y+3.3f,transform.position.z), Color.blue); 


			foreach (RaycastHit hit in hits)
			{           
				Debug.Log (hit.transform.name);

				if (hit.transform.tag == "enemy")
					continue;

				if (hit.transform.tag != "snake")
				{
					Debug.Log ("between");
					return true;
				}
			}

		
			return false; 

		}






			
		//shooting
		/*void Shoot(){

			if (onRange){

				Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
				bullet.AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);

				Destroy (bullet.gameObject, 2);
			}


		}

		//shooting
		void Update() {

			onRange = Vector3.Distance(transform.position, player.position)<range;

			if (onRange)
				transform.LookAt(player);
		}*/



    }

}