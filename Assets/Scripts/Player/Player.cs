using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour, IPausable
{
    [Header("Required Reference")]
    [SerializeField] GuiController guiPrefab;
    [SerializeField] new Camera camera;

    public GuiController Gui { get; private set; }
    public bool IsPaused { get; private set; }
    private IPausable[] pausableComponents;

    public override void OnStartClient()
    {
        base.OnStartClient();

        camera.enabled = false;
        camera.GetComponent<AudioListener>().enabled = false;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        
        camera.enabled = true;
        camera.GetComponent<AudioListener>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;

        Gui = Instantiate(guiPrefab);
        pausableComponents = this.GetComponentsInChildren<IPausable>();

        ClientController.Instance.RegisterClientPlayer(this);
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
