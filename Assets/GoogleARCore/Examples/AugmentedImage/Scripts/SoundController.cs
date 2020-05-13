using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource wordClip;
    // Start is called before the first frame update
    void Start()
    {
        wordClip= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log("Something Hit");
                if (raycastHit.collider.name == "wordPrefab")
                {
                    Debug.Log("Soccer Ball clicked");
                    wordClip.Play();
                }
            }
        }
    }

}
