using UnityEngine;
using System.Collections;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
		public int turboForce = 30;

		public PdComClass pdCom;

        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.

		private bool m_TurboInput; 					// Check if t is pressed
		private float StartOfTurboMode;
		private float lastTurboUse = 0;

        private float startOfEngineRunning;
        private bool tankRunning;
        private float noiseVariable;

        private float nextActionTime = 0.0f;
        public float period = 0.1f;

        private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();
        }


        private void OnEnable ()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;
            pdCom.send("Start " + 0.5);
            
            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;
        }


        private void OnDisable ()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;
            pdCom.send("Stop");
        }


        private void Start ()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;
            InvokeRepeating("DoThrottleDown", 0.5f, 0.2F);
            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;
        
			//Set the refernec to sound class
			pdCom = FindObjectOfType(typeof(PdComClass)) as PdComClass;
            pdCom.send("Start " + 0.5);
            tankRunning = false;
        }


        private void Update ()
        {
            // Store the value of both input axes.
            m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
			m_TurboInput = Input.GetKey(KeyCode.T);
            EngineAudio ();
			if (Input.GetKeyDown (KeyCode.T)) {
				StartOfTurboMode = Time.time;
			}
        }   

        private void EngineAudio ()
        { 
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {	
                tankRunning = false;
            }
            else
            {
                if(!tankRunning)
                { 
                    startOfEngineRunning = Time.time - 1;
                    tankRunning = true;
                    pdCom.send("stop");
                    
                }else {
                    noiseVariable = (Time.time - startOfEngineRunning); 
                    if(noiseVariable < 1){
                        pdCom.send("Speed " + 1);
                    }else if(noiseVariable < 3){
                        pdCom.send("Speed " + noiseVariable);
                    } else {
                        pdCom.send("Speed " + 3);
                    }
                }
            }
        }


        private void FixedUpdate ()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move ();
            Turn ();
			ApplyTurboForce();
        }


        private void Move ()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn ()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
        }

		private void ApplyTurboForce () {
			if (m_TurboInput && (Time.time - StartOfTurboMode) < 0.5f && Input.GetKey(KeyCode.W)) {
				Vector3 movement = transform.forward * turboForce;
				m_Rigidbody.AddForce(movement);
			} 
			if ((Time.time - StartOfTurboMode) > 1 && (Time.time - lastTurboUse) > 10){
				lastTurboUse = Time.time;
			}
		}

        private void DoThrottleDown (){
            Debug.Log("doThrottleDown called -->");
            if(!tankRunning){
                if (noiseVariable > 3 ){
                    noiseVariable = 3;
                } else if (noiseVariable < 1.0f){
                    noiseVariable = 1.0f;
                }
                if (noiseVariable > 1.0f){
                    noiseVariable = noiseVariable-0.5f;
                }
                pdCom.send("Speed " + noiseVariable);
                Debug.Log("doThrottleDown in if statement -->");
            }
        }

    }
}