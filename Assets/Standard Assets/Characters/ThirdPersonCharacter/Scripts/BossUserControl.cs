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
        private Vector3 m_Move;
        private float end_p;
        public bool move = true;
        private float start_p;
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
        void Shoot()
        {
            Debug.Log("Shoot");
            m_Move = new Vector3(0, 0, 0);
            m_Character.Move(m_Move, false, false);
            m_Character.getAnimator().SetBool("shoot", true);
       

            if (onRange)
            {

                Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
                bullet.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);

                Destroy(bullet.gameObject, 2);
            }
            return;
        }

		void OnCollisionEnter(Collision other)
		{
			if (other.transform.tag == "bullet") {
				Destroy (other.gameObject);
				health -= 20;
			}
		}


    }
}