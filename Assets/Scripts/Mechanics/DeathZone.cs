using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;


namespace Platformer.Mechanics
{
    public enum LevelEnum
    {
        Level1,
        Level2,
        Level3
    }
    
    /// <summary>
    /// DeathZone components mark a collider which will schedule a
    /// PlayerEnteredDeathZone event when the player enters the trigger.
    /// </summary>
    public class DeathZone : MonoBehaviour
    {
        public List<LevelEnum> ActiveLevels = new List<LevelEnum>();
        
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!ActiveLevels.Contains(GameController.Instance.GetCurrentLevel())) return;
            
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Schedule<PlayerEnteredDeathZone>();
                ev.deathzone = this;
            }
        }
    }
}