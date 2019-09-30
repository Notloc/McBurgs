using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerBuilder : MonoBehaviour
{

    [SerializeField] BurgerNode startNode;
    [SerializeField] float animationLength = 0.4f;
    [SerializeField] [Range(0.1f,1.5f)] float animationStrength = 0.1f;

    BurgerNode activeNode;
    List<IBurgerComponent> burgerComponents;

    private void Awake()
    {
        burgerComponents = new List<IBurgerComponent>();
        activeNode = startNode;
    }

    private void OnTriggerEnter(Collider other)
    {
        var node = other.GetComponent<BurgerNode>();
        if (node)
            Attach(node);
    }

    private void Attach(BurgerNode node)
    {
        var component = node.GetComponentInParent<IBurgerComponent>();
        if (InterfaceUtil.IsNull(component))
            return;

        burgerComponents.Add(component);

        activeNode.Disable();
        activeNode = component.AttachTo(activeNode);

        if (!activeNode)
            FinishBurger();

        StartCoroutine(AttachAnimation());
    } 

    private IEnumerator AttachAnimation()
    {
        activeNode.Disable();

        int i = 0;
        float[] originalOffsets = new float[burgerComponents.Count];

        // Cache all the original offset values
        foreach (IBurgerComponent component in burgerComponents)
        {
            originalOffsets[i] = component.transform.localPosition.y;
            i++;
        }


        float startTime = Time.time;
        while (startTime + animationLength > Time.time)
        {
            i = 0;
            foreach (IBurgerComponent component in burgerComponents)
            {
                Vector3 pos = component.transform.localPosition;

                float progress =  (Time.time - startTime) / animationLength;
                pos.y = originalOffsets[i] - (animationStrength * Mathf.Sin(progress * Mathf.PI));

                component.transform.localPosition = pos;
                i++;
            }
            yield return null;
        }

        i = 0;
        foreach (IBurgerComponent component in burgerComponents)
        {
            Vector3 pos = component.transform.localPosition;
            pos.y = originalOffsets[i];
            component.transform.localPosition = pos;
            i++;
        }

        if (activeNode)
            activeNode.Enable();
    }

    private void FinishBurger()
    {

    }
}
