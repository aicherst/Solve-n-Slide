//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(CapsuleCollider))]
//public class HoverHomingRocket : MonoBehaviour {
//    public float maxSpeed = 1;
//    public AnimationCurve accelerationCurve;
//    public float rotationSpeed = 10;              // degree per sec

//    public float sidewaysSpeed = 0.1f;

//    public float buffer = 0.5f;

//    public float groundDistance = 3;

//    public Transform target;

//    private CapsuleCollider capsuleCollider;

//    private float lifeTime;

//    // Use this for initialization
//    void Start() {
//        lifeTime = 0;

//        capsuleCollider = GetComponent<CapsuleCollider>();
//    }

//    // Update is called once per frame
//    void Update() {
//        lifeTime += Time.deltaTime;

//        Vector3 targetDirection = (target.position - transform.position).normalized;

//        //find the vector pointing from our position to the target
//        Vector3 rotationDirection = targetDirection;

//        if (!InSight(targetDirection)) {
//            Collider[] colliders = Physics.OverlapSphere(transform.position, groundDistance, Layers.terrain);
//            if (colliders.Length > 0) {
//                RaycastHit hit;
//                if (Physics.Raycast(transform.position, targetDirection, out hit)) {
//                    string tag = hit.collider.tag;

//                    if (tag == Tags.terrain) {
//                        Vector3 cross = Vector3.Cross(hit.normal, targetDirection);

//                        rotationDirection = Quaternion.AngleAxis(90, cross) * hit.normal;
//                    }
//                }
//            }
//        } else {
//            Debug.Log("InSight");
//        }





//        //create the rotation we need to be in to look at the target
//        Quaternion lookRotation = Quaternion.LookRotation(rotationDirection);

//        //rotate us over time according to speed until we are in the required rotation
//        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);


//        Vector3 veclocity = lookRotation * Vector3.forward * accelerationCurve.Evaluate(lifeTime) * maxSpeed;

//        transform.position += veclocity * Time.deltaTime;



//    }

//    private bool InSight(Vector3 targetDirection) {
//        {
//            RaycastHit hit;
//            if (Physics.Raycast(transform.position, targetDirection, out hit)) {
//                string tag = hit.collider.tag;

//                if (tag == Tags.terrain) {
//                    return false;
//                }
//            }
//        }

//        {
//            RaycastHit hit;
//            if (Physics.SphereCast(transform.position, groundDistance, targetDirection, out hit)) {
//                string tag = hit.collider.tag;

//                if (tag == Tags.terrain) {
//                    return false;
//                }
//            }
//        }


//        return true;
//    }


//    private void OnTriggerEnter(Collider other) {
//        if (other.tag == Tags.terrain) {
//            enabled = false;
//            //Destroy(gameObject);
//        }
//    }

//    private void OnDrawGizmos() {
//        Vector3 targetDirection = (target.position - transform.position).normalized;

//        //find the vector pointing from our position to the target
//        Vector3 rotationDirection = targetDirection;

//        if (!InSight(targetDirection)) {
//            Collider[] colliders = Physics.OverlapSphere(transform.position, groundDistance, Layers.terrain);
//            if (colliders.Length > 0) {
//                RaycastHit hit;
//                if (Physics.Raycast(transform.position, targetDirection, out hit)) {
//                    string tag = hit.collider.tag;

//                    if (tag == Tags.terrain) {
//                        Vector3 cross = Vector3.Cross(hit.normal, targetDirection);

//                        rotationDirection = Quaternion.AngleAxis(90, cross) * hit.normal;
//                    }
//                }

//                Gizmos.color = Color.red;
//                Gizmos.DrawWireSphere(transform.position, groundDistance);
//            } else {
//                Gizmos.color = Color.green;
//                Gizmos.DrawWireSphere(transform.position, groundDistance);
//            }


//            Gizmos.color = Color.red;
//            Gizmos.DrawLine(transform.position, target.transform.position);
//        } else {
//            Gizmos.color = Color.green;
//            Gizmos.DrawLine(transform.position, target.transform.position);
//        }


//    }
//}
