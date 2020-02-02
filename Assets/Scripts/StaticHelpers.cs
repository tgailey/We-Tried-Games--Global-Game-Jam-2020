using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class StaticHelpers
{
    public static List<GameObject> GetAllConnectedGameObjects(GameObject startingGO)
    {
        List<GameObject> lookedAtObjects = new List<GameObject>();
        Queue<GameObject> toLookAtObjects = new Queue<GameObject>();

        toLookAtObjects.Enqueue(startingGO);
        lookedAtObjects.Add(startingGO);

        while (toLookAtObjects.Count > 0)
        {
            GameObject obj = toLookAtObjects.Dequeue();

            FixedJoint[] joints = obj.GetComponents<FixedJoint>();
            foreach (FixedJoint joint in joints)
            {
                if (joint.connectedBody != null && lookedAtObjects.Contains(joint.connectedBody.gameObject) == false)
                {
                    toLookAtObjects.Enqueue(joint.connectedBody.gameObject);
                    lookedAtObjects.Add(joint.connectedBody.gameObject);
                }
            }

            RelationshipNoter[] relationships = obj.GetComponents<RelationshipNoter>();
            foreach (RelationshipNoter relationship in relationships)
            {
                if (lookedAtObjects.Contains(relationship.relationshipObject) == false)
                {
                    toLookAtObjects.Enqueue(relationship.relationshipObject);
                    lookedAtObjects.Add(relationship.relationshipObject);
                }
            }
        }

        return lookedAtObjects;
    }
}
