using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class KillBelow : MonoBehaviour
{
    public float killBelow = 0;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < killBelow)
        {
            if(GetComponent<Player>())
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else
                Destroy(gameObject);
        }
    }
}
