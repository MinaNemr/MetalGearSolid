using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class BossUserControl : MonoBehaviour
    {
        public bool playerInRange;

        [SerializeField]
        Transform player; 
        public float range = 50.0f;
        public float bulletImpulse = 20.0f;
        private bool onRange = false;
        public Rigidbody projectile;
		int health = 200;
		public Text health_t;
        private ThirdPersonCharacter m_Character; 
		public ThirdPersonCharacter target;
        private Vector3 m_Move;
        private float end_p;
        public bool move = true;
        private float start_p;
		Rigidbody bullet;
        private void Start()
        {
            m_Character = GetComponent<ThirdPersonCharacter>();
            player = GameObject.Find("Snake").transform;
            InvokeRepeating("Hide", 5.0f, 7.0f);
            InvokeRepeating("Shoot", 0.0f, 5.0f);

        }


        void Update()
        {
			
			health_t.text = "Boss Health: " + health;
            if (m_Character.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
            {
                m_Character.getAnimator().SetBool("shoot", false);
            }
			if (m_Character.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Hit"))
			{
				m_Character.getAnimator().SetBool("hit", false);
			}
            onRange = Vector3.Distance(transform.position, player.position) < range;
           if (onRange)
                transform.LookAt(player);

        }
        void Hide()
        {
            m_Character.getAnimator().SetBool("shoot", false);
            Debug.Log("Hide");
            m_Move = new Vector3(0, 0, 0);
            m_Character.Move(m_Move, true, false);
            return;
        }
		bool bulletIn()
		{

			Vector3 directionToPlayer = bullet.position - player.position; 
			//Debug.DrawLine(transform.position, player.position, Color.magenta); 

			Vector3 lineOfSight = bullet.position - player.position; 
			//Debug.DrawLine(transform.position, lineOfSightEnd.position, Color.yellow); 


			float angle = Vector3.Angle(directionToPlayer, lineOfSight);

			if (angle < 65 && Vector3.Distance(bullet.position,player.position)<10) {
				return true;
			}
			else {
				return false;
			}


		}
        void Shoot()
        {
            
            m_Move = new Vector3(0, 0, 0);
            m_Character.Move(m_Move, false, false);
			if (onRange)
			{
				RaycastHit hit;
				Vector3 fwd = transform.TransformDirection(Vector3.forward);
				if (Physics.Raycast (new Vector3(transform.position.x,transform.position.y+3.2f,transform.position.z), fwd, out hit, 100.0f)) {
					if (hit.transform.tag == "snake") {
						Debug.Log ("Detected");
						Vector3 offset = new Vector3 (0.0f,1.0f,0.7f);
						Transform bulletPlace = GameObject.Find("Mp7").transform;
						//Vector3 dir =new Vector3 (hit.transform.position.x+3.0f, hit.transform.position.y + 7.2f, hit.transform.position.z) - new Vector3 (transform.position.x, transform.position.y + 3.3f, transform.position.z);
						 bullet = (Rigidbody)Instantiate(projectile, bulletPlace.position+offset, Quaternion.LookRotation(new Vector3 (hit.transform.position.x, hit.transform.position.y + 3.2f, hit.transform.position.z) - new Vector3 (transform.position.x, transform.position.y + 3.3f, transform.position.z)));
						bullet.AddForce((transform.forward) * bulletImpulse*0.5f, ForceMode.Impulse);
						if(bulletIn()){
							target.getAnimator().SetBool("hit", true);
							target.setHealth(target.getHealth()-20);
						}
						Destroy(bullet.gameObject, 2);
					}
				}

				/*Vector3 relativePos = player.position - transform.position;
				Quaternion rotation = Quaternion.LookRotation(relativePos);
				transform.rotation = rotation;
				Vector3 offset = new Vector3(-0.5f, 1, 0);
				Transform bulletPlace = GameObject.Find("Mp7").transform;
				Rigidbody bullet = (Rigidbody)Instantiate(projectile, bulletPlace.position + transform.forward+offset , bulletPlace.rotation);
				bullet.AddForce(transform.forward * bulletImpulse*0.5f, ForceMode.Impulse);
				Destroy(bullet.gameObject, 2);*/
			}
            m_Character.getAnimator().SetBool("shoot", true);
       


            return;
        }

		void OnCollisionEnter(Collision other)
		{
			if (other.transform.tag == "bullet") {
				Destroy (other.gameObject);
				m_Character.getAnimator().SetBool("hit", true);
				health -= 20;

			}
		}


    }
}