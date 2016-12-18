using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class BossUserControl : MonoBehaviour
    {
        public bool playerInRange;

        [SerializeField]
        Transform lineOfSightEnd;

        //shooting
        Transform player; //common
        public Rigidbody projectile;

        private ThirdPersonCharacter m_Character; 
        private Vector3 m_Move;
        private float end_p;
        public bool move = true;
        private float start_p;
        private void Start()
        {
            m_Character = GetComponent<ThirdPersonCharacter>();
            playerInRange = false;
            player = GameObject.Find("Snake").transform;
            lineOfSightEnd = GameObject.Find("view").transform;
            m_Character.setCurrent(m_Character.transform.position);
            InvokeRepeating("Hide", 5.0f, 7.0f);
            InvokeRepeating("Shoot", 0.0f, 5.0f);
        }


        void Update()
        {
            
            
        }
        void Hide()
        {
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
            return;
        }
    }
}