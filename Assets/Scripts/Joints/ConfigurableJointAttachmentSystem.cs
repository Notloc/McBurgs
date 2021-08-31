using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableJointAttachmentSystem : JointConnectionSystem
{
    [SerializeField] ConfigurableJointSettings jointSettings = new ConfigurableJointSettings();

    protected override Joint CreateJoint()
    {
        ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
        jointSettings.Apply(joint);
        return joint;
    }
}
