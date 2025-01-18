using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    float BulletAliveTime;
    private GameBehaviour _gameManager;
    void Start()
    {
        _gameManager=GameObject.Find("Game_Manager").GetComponent<GameBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name=="Player"){
            _gameManager.HP=_gameManager.HP-1;
            Destroy(gameObject);
            Debug.Log("Player gets hit!");
        }
        else{
            Destroy(gameObject);
        }
    }
}
