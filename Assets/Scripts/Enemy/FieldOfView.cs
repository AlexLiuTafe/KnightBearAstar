using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    #region Variables
    [Range(2, 20)]
    public float viewRadius; //how big our circle of detection is //size of the circle
    [Range(0, 360)]
    public float viewAngle; // this limits the size of the angle (field of vision)
    public float viewAngleDirection; //this value determines direction of the start poit of the field of view (left or right)
    public bool targetIsVisible;

    public LayerMask WendysTargetLayer; // get all targets from this layer
    public LayerMask obstacles; // get all obstacles / walls from this layer

    // list for the targets that are visible at any moment(checks every 0.2 seconds)
    public List<Transform> visibleTargetList = new List<Transform>();
    #endregion


    void Start()
    {
        // we want to check every 0.2 seconds (adjust as needed)
        StartCoroutine("FindTargetsWithDelay", 0.2f);
        
    }

    #region IEnumerator: FindTargetsWithDelay
    // this function is to set up our delay wait time for the method WaitForSeconds to run
    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay); // sets the delay
            FindVisibleTargets(); // method to check for targets
        }
    }
    #endregion


    #region Method: FindVisibleTargets
    //this method will get a list of valid targets that fall in the range of 
    //the limits we set on the "field of vision
    //it does 4 things:
    //  Uses OverlapSphere to return a list of colliders (only checks in the 'target' layer')
    //   
    //  Go through that list one by one
    //      Checks if its within view angle specified
    //          Checks if there is an obstacle in the path
    void FindVisibleTargets()
    {
        visibleTargetList.Clear();
        // get all target colliders in the overlap sphere and put them in our collider array called targetsInViewRadius
        //this code is for 3d
        //Collider[] targetsinViewRadius = Physics.OverlapSphere(transform.position, viewRadius, WendysTargetLayer);
        //2D version
        Collider2D[] targetsinViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, WendysTargetLayer);
        Debug.Log("Who the fuck is there????" + targetsinViewRadius.Length);

        // Go through that list one by one
        for (int i = 0; i < targetsinViewRadius.Length; i++)
        {
            Transform target = targetsinViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            //Checks if target is within view angle specified
            if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                //the target is with in the view angle but is there an obstacle?
                //get distance player to the current target
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                //do a raycast and check for a false result (not true)
                if (!Physics2D.Raycast(transform.position, dirToTarget, distanceToTarget, obstacles))
                {
                    // raycast did not his anything
                    //add the current target to the list of found targets
                    //start the A* 2D pathing here
                    //set inLineOfSight to true (for attacks)
                    visibleTargetList.Add(target);
                    targetIsVisible = true;
                    Debug.Log("TRUE!!!!!!!");
                    //can add other funtionality here depending on need
                    //or use list in ohter functions

                }
                else
                {
                    targetIsVisible = false;
                    Debug.Log("FALSE!!!!!!!");
                }

            }
        }
    }
    #endregion


    //this funtion is used to return a direction from an angle we supply to it
    //only used for drawing visualisation lines in the editor (scene view)
    //these lines will represent the boundary of our field of vision.
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            //makes the angle local (relative the object this script is attached to
            angleInDegrees += transform.eulerAngles.x;
        }
        return new  Vector3(Mathf.Sin((angleInDegrees + viewAngleDirection) * Mathf.Deg2Rad), Mathf.Cos((angleInDegrees + viewAngleDirection) * Mathf.Deg2Rad), 0);
    }
}
