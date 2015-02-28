// http://www.phase-digital.com/tutorials/u3d/unity3d-tutorial-transparency-objects-camera-view-rpg-games/

using UnityEngine;
using System.Collections;
 
public class CameraRaycast : MonoBehaviour
{
    public GameObject Player;
    GameObject BlockObject;
    RaycastHit hit;
    Ray ray;
    Material oldShader;
    // Use this for initialization
    void Start()
    {
    }
 
    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(Player.transform.position.x + xOffset, Player.transform.position.y + yOffset, Player.transform.position.z + zOffset);
        //RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
        {
            //Debug.Log ("Camera's Raycast hit " + hit.collider);
            if (hit.collider != Player.gameObject.collider)
            {
                //Debug.Log ("Camera's Raycast hit something that's not a player!");
                Renderer R = hit.collider.renderer;
                // if (R == null)
                //    continue; // no renderer attached? go to next hit
                // TODO: maybe implement here a check for GOs that should not be affected like the player
                if (R != null)
                {
                    AutoTransparent AT = R.GetComponent<AutoTransparent>();
                    if (AT == null) // if no script is attached, attach one
                    {
                        AT = R.gameObject.AddComponent<AutoTransparent>();
                    }
                    AT.BeTransparent(); // get called every frame to reset the falloff
                }
 
            }
        }
    }
}