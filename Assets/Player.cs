using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPausable
{
    public bool IsPaused { get; private set; }
    private IPausable[] pausableComponents;

    private void Awake()
    {
        pausableComponents = this.GetComponentsInChildren<IPausable>();
    }

    public void Pause()
    {
        if (IsPaused)
            return;

        IPausable self = this as IPausable;
        foreach (IPausable component in pausableComponents)
            if (component != self)
                component.Pause();

        IsPaused = true;
    }

    public void Unpause()
    {
        if (!IsPaused)
            return;

        IPausable self = this as IPausable;
        foreach (IPausable component in pausableComponents)
            if (component != self)
                component.Unpause();

        IsPaused = false;
    }
}
