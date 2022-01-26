using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrankieCamarilloDeleteOther : MonoBehaviour{
	
	public LayerMask enemyLayer; // this script only takes on layer
	public GameObject boomVFX;
    private ContactPoint2D[] contacts = new ContactPoint2D[10];

    // void OnCollisionEnter(Collision collision)
    // {
        //test
        // foreach (ContactPoint contact in collision.contacts)
        // {
            //object is in enemy layer
            // if (contact.otherCollider.gameObject.layer == 6)
                // Destroy(contact.otherCollider.gameObject);
        // }
    // }
	
	
	public void OnTriggerEnter2D(Collider2D col){
		float layerNumber = Mathf.Log(enemyLayer.value, 2);
		//Debug.Log("I see an enemy: " + col + " " + col.gameObject.layer + " " + layerNumber);
					
		if (col.gameObject.layer == layerNumber){			
			Vector2 SpawnBoomPos = col.gameObject.transform.position;
			GameObject boom = Instantiate (boomVFX, SpawnBoomPos, Quaternion.identity);
			StartCoroutine(DestroyVFX(boom));
            
			Destroy(col.gameObject);
			
		}
    }
	
	IEnumerator DestroyVFX(GameObject thisBoom){
		yield return new WaitForSeconds(0.5f);
		Destroy(thisBoom);
	}
	
}
