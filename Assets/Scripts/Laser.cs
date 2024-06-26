using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _laserSpeed = 8.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if(transform.position.y > 8f)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
