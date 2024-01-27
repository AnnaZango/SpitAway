using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //We create an extension for the Rigidbody2D to check for collisions. We create an extension because
    //we want to use the this code multiple times.

    private static LayerMask layerMask = LayerMask.GetMask("Default");

    public static bool RaycastFirstHit(this Rigidbody2D rigidbody, Vector2 direction, float radius, float distance,
        LayerMask layerToDetect)
    {
        if (rigidbody.isKinematic) { return false; }

        //we cast a circle and return if we hit something

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction, distance,
            layerToDetect);
       
        Vector3 endRay = new Vector3(rigidbody.transform.position.x + (direction.x * distance),
            rigidbody.transform.position.y + (direction.y * distance), 
            rigidbody.transform.position.z);

        Debug.DrawLine(rigidbody.transform.position, endRay, Color.red);        

        if(hit.collider != null && hit.rigidbody != rigidbody)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool RaycastRayFirst(this Rigidbody2D rigidbody, Vector2 direction, float distance,
        LayerMask layer)
    {
        if (rigidbody.isKinematic) { return false; }

        RaycastHit2D hitRay = Physics2D.Raycast(rigidbody.position, direction.normalized, distance,
            layer);

        RaycastHit2D[] hitsRay = Physics2D.RaycastAll(rigidbody.position, direction.normalized,
            distance, layer);

        Vector3 endRay = new Vector3(rigidbody.transform.position.x + (direction.normalized.x * distance),
            rigidbody.transform.position.y + (direction.normalized.y * distance),
            rigidbody.transform.position.z);

        Debug.DrawLine(rigidbody.transform.position, endRay, Color.green);

        if (hitRay.collider != null && hitRay.rigidbody != rigidbody)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static List<GameObject> RaycastAll(this Rigidbody2D rigidbody, Vector2 direction, float radius, float distance,
        LayerMask layerToDetect)
    {
        if (rigidbody.isKinematic) { return null; }

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance,
            layerToDetect);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(rigidbody.position, radius,
            direction.normalized, distance);

        Vector3 endRay = new Vector3(rigidbody.gameObject.transform.position.x + (direction.x * distance),
            rigidbody.gameObject.transform.position.y + (direction.y * distance),
            rigidbody.transform.position.z);

        Debug.DrawLine(rigidbody.gameObject.transform.position, endRay, Color.red);
       
        List<GameObject> objectsHit = new List<GameObject>();

        foreach (RaycastHit2D collisionHit in hits)
        {
            if(collisionHit.rigidbody == rigidbody)
            {
                //Debug.Log("My own collision is: " + collisionHit.collider.gameObject.name);
            }
            else
            {
                objectsHit.Add(collisionHit.collider.gameObject);
            }            
        }

        return objectsHit;
    }



}
