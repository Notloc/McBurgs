using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HingeJointSettings
{
    [SerializeField] bool enableCollision;

    [Header("Anchors")]
    [SerializeField] bool autoConfigureAnchor;
    [SerializeField] Vector3 connectedAnchorPosition;
    [SerializeField] Vector3 anchorPosition;

    [Header("Spring")]
    [SerializeField] bool useSpring;
    [SerializeField] float springForce = 0f;
    [SerializeField] float springDamper = 0f;

    [Header("Break")]
    [SerializeField] float breakForce = float.PositiveInfinity;

    public void ApplySettings(HingeJoint hingeJoint, float multiplier)
    {
        hingeJoint.autoConfigureConnectedAnchor = autoConfigureAnchor;
        if (!autoConfigureAnchor)
        {
            hingeJoint.connectedAnchor = connectedAnchorPosition;
            hingeJoint.anchor = anchorPosition;
        }
        hingeJoint.enableCollision = enableCollision;

        JointSpring jointSpring = new JointSpring();
        jointSpring.spring = springForce * multiplier;
        jointSpring.damper = springDamper * multiplier;
        hingeJoint.spring = jointSpring;
        hingeJoint.useSpring = useSpring;

        hingeJoint.breakForce = breakForce * multiplier; ;
    }
}
