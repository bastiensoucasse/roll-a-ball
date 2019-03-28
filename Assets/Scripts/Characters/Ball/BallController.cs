using System;
using UnityEngine;

namespace Profuder.Games.Characters.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        [SerializeField]
        private float m_ForwardSpeed = 6.0f;
        [SerializeField]
        private float m_BackSpeed = 3.0f;
        [SerializeField]
        private float m_SidesSpeed = 2.1f;
        [Range(2, 6)]
        [SerializeField]
        private int m_SprintMultiplier = 3;
        [SerializeField]
        private float m_JumpPower = 3.0f;
        [SerializeField]
        private float m_VisionSensivity = 3.0f;

        private Camera m_Camera;
        private Vector3 m_CameraOffset;

        private Rigidbody m_Rigidbody;
        private bool m_Grounded;
        private bool m_Crouching;

        /// <summary>
        /// What to do when launching the game.
        /// </summary>
        private void Start()
        {
            m_Camera = Camera.main;
            m_Rigidbody = GetComponent<Rigidbody>();

            m_CameraOffset = m_Camera.transform.position - this.transform.position;
            m_Grounded = true;
            m_Crouching = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void FixedUpdate()
        {
            Move();
            Jump();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LateUpdate()
        {
            MoveCamera();
            RotateCamera();
        }

        /// <summary>
        /// Set the ball to grounded when hitting the floor.
        /// </summary>
        /// <param name="collision">The collision triggered.</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Floor")) m_Grounded = true;
        }

        /// <summary>
        /// Move the ball horizontaly and verticaly.
        /// </summary>
        public void Move()
        {
            Vector3 move = Vector3.zero;
            var OVR = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

            move.x = Input.GetAxis("Horizontal") + OVR.x;
            move.z = Input.GetAxis("Vertical") + OVR.y;

            if (Input.GetButtonDown("Crouch")) m_Crouching = !m_Crouching;

            if (Input.GetButton("Sprint") || Input.GetAxis("Sprint") > 0)
            {
                if (m_Crouching) m_Crouching = false;
                move *= m_SprintMultiplier;
            }

            if (!m_Crouching)
            {
                move.x *= m_SidesSpeed;
                if (move.z >= 0) move.z *= m_ForwardSpeed;
                else move.z *= m_BackSpeed;
            }

            move = m_Camera.transform.TransformDirection(move);
            m_Rigidbody.AddForce(move);
        }

        /// <summary>
        /// Impulse up the ball when the jump button is pressed.
        /// </summary>
        public void Jump()
        {
            if ((Input.GetButtonDown("Jump") || OVRInput.GetDown(OVRInput.Button.Any)) && m_Grounded)
            {
                Vector3 jump = Vector3.zero;

                jump.y = 30;

                GetComponent<Rigidbody>().AddForce(jump * m_JumpPower);
                m_Grounded = false;
            }
        }

        /// <summary>
        /// Move the camera to follow the ball.
        /// </summary>
        private void MoveCamera()
        {
            m_Camera.transform.position = this.transform.position + m_CameraOffset;
            m_Camera.transform.LookAt(this.transform.position);
        }

        /// <summary>
        /// Rotate the camera on y when moving the vision.
        /// </summary>
        private void RotateCamera()
        {
            m_CameraOffset = Quaternion.AngleAxis(Input.GetAxis("Vision X") * m_VisionSensivity, Vector3.up) * m_CameraOffset;
        }
    }
}
