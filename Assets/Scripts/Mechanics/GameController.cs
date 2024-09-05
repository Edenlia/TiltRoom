using System.Collections.Generic;
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

        public GameObject Level1BG;
        public GameObject Level2BG;
        public GameObject Level3BG;
        
        public LevelTrigger LevelTrigger12;
        public LevelTrigger LevelTrigger23;

        public GameObject EnemyRoot;
        
        public GameObject NoiseCanvas;
        
        public List<GameObject> Level3RotateObjects;
        
        private AudioClip _noiseWave;
        private AudioSource _audioSource;
        
        private LevelEnum _currentLevel = LevelEnum.Level1;
        
        private bool _level3Rotating = false;
        

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public void Restart()
        {
            _audioSource.clip = LevelSound1;
            _audioSource.Play();
    
            Level1.SetActive(true);
            Level2.SetActive(false);
            Level3.SetActive(false);
            
            Level1BG.SetActive(true);
            Level2BG.SetActive(false);
            Level3BG.SetActive(false);
            
            LevelTrigger12.Restart();
            LevelTrigger23.Restart();
            
            EnemyRoot.SetActive(false);
            
            Level3RotateObjects.ForEach(obj => obj.transform.rotation = Quaternion.identity);
            _level3Rotating = false;
        }

        void OnEnable()
        {
            Instance = this;
            
            EnemyRoot.SetActive(false);
            
            Level2.SetActive(false);
            Level3.SetActive(false);
            
            Level1BG.SetActive(true);
            Level2BG.SetActive(false);
            Level3BG.SetActive(false);

            _noiseWave = Resources.Load<AudioClip>("Audio/NoiseWave");
            _audioSource = gameObject.GetComponent<AudioSource>();
            _audioSource.clip = LevelSound1;
            _audioSource.Play();
            
            // TODO: Test, need delete
            // LoadLevel2();
            // LoadLevel3();
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (_level3Rotating)
            {
                float delta = Time.deltaTime;
                float angle = 0.1f * delta;
                foreach (var obj in Level3RotateObjects)
                {
                    obj.transform.Rotate(0, 0, angle);
                }
            }
            
            if (Instance == this) Simulation.Tick();
        }
        
        public void LoadLevel2()
        {
            _currentLevel = LevelEnum.Level2;
            EnemyRoot.SetActive(true);
            Level1.SetActive(false);
            Level2.SetActive(true);
            Level1BG.SetActive(false);
            Level2BG.SetActive(true);
        }
        
        public void LoadLevel3()
        {
            _currentLevel = LevelEnum.Level3;
            Level2.SetActive(false);
            Level3.SetActive(true);
            _level3Rotating = true;
            Level2BG.SetActive(false);
            Level3BG.SetActive(true);
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

        public LevelEnum GetCurrentLevel()
        {
            return _currentLevel;
        }
    }
}