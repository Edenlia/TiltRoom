using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        public GameObject Level1;
        public GameObject Level2;
        public GameObject Level3;

        public AudioClip LevelSound1;
        public AudioClip LevelSound2;
        public AudioClip LevelSound3;

        public GameObject EnemyRoot;
        
        public GameObject NoiseCanvas;
        
        private AudioClip _noiseWave;
        private AudioSource _audioSource;
        
        
        

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        void OnEnable()
        {
            Instance = this;
            
            Level2.SetActive(false);
            Level3.SetActive(false);

            _noiseWave = Resources.Load<AudioClip>("Audio/NoiseWave");
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = LevelSound1;
            _audioSource.Play();
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }
        
        public void LoadLevel2()
        {
            EnemyRoot.SetActive(true);
            Level1.SetActive(false);
            Level2.SetActive(true);
        }
        
        public void LoadLevel3()
        {
            Level2.SetActive(false);
            Level3.SetActive(true);
        }

        public void StartNoise(int ToLevel)
        {
            NoiseCanvas.SetActive(true);
            _audioSource.clip = _noiseWave;
            _audioSource.Play();
        }
        
        public void StopNoise(int ToLevel)
        {
            NoiseCanvas.SetActive(false);
            if (ToLevel == 2)
                _audioSource.clip = LevelSound2;
            else if (ToLevel == 3)
                _audioSource.clip = LevelSound3;
            _audioSource.Play();
        }
    }
}