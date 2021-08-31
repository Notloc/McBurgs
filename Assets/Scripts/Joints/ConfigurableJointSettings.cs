using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigurableJointSettings
{
    [Header("Anchors")]
    [SerializeField] bool autoConfigureAnchor;
    [SerializeField] Vector3 connectedAnchorPosition;
    [SerializeField] Vector3 anchorPosition;

    [SerializeField] bool enableCollision;

    [Header("Linear Joint")]
    [SerializeField] float linearLimit = 0f;
    [SerializeField] float bounciness = 0.5f;

    [Header("Linear Spring")]
    [SerializeField] float springForce = 0f;
    [SerializeField] float springDamper = 0f;



    [SerializeField] float breakForce = float.PositiveInfinity;


    public void Apply(ConfigurableJoint configJoint)
    {
        Apply(configJoint, 1f);
    }

    public void Apply(ConfigurableJoint configJoint, float multiplier)
    {
        configJoint.autoConfigureConnectedAnchor = autoConfigureAnchor;
        if (!autoConfigureAnchor)
        {
            configJoint.connectedAnchor = connectedAnchorPosition;
            configJoint.anchor = anchorPosition;
        }
        configJoint.enableCollision = enableCollision;

        configJoint.xMotion = ConfigurableJointMotion.Limited;
        configJoint.yMotion = ConfigurableJointMotion.Limited;
        configJoint.zMotion = ConfigurableJointMotion.Limited;

        configJoint.angularXMotion = ConfigurableJointMotion.Limited;
        configJoint.angularYMotion = ConfigurableJointMotion.Limited;
        configJoint.angularZMotion = ConfigurableJointMotion.Limited;

        SoftJointLimitSpring springSettings = new SoftJointLimitSpring();
        springSettings.spring = springForce * multiplier;
        springSettings.damper = springDamper * multiplier;
        configJoint.linearLimitSpring = springSettings;

        SoftJointLimit jointSettings = new SoftJointLimit();
        jointSettings.limit = linearLimit;
        jointSettings.bounciness = bounciness;
        configJoint.linearLimit = jointSettings;

        configJoint.breakForce = breakForce * multiplier;
    }
}
