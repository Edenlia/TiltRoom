using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public int ToLevel = 2;

    public void Restart()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("An object entered the trigger: " + other.name);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has found the treasure!");
            // 添加更多逻辑，比如打开宝箱动画或增加分数
            if (ToLevel == 2)
                GameController.Instance.LoadLevel2();
            else if (ToLevel == 3)
                GameController.Instance.LoadLevel3();
            StartCoroutine(NoiseCoroutine());
            
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }
    }
    
    private IEnumerator NoiseCoroutine()
    {
        GameController.Instance.StartNoise(ToLevel);
        yield return new WaitForSeconds(0.3f);
        GameController.Instance.StopNoise(ToLevel);
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
