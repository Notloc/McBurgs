using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OrderTrayTest
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator OrderTrayTestWithEnumeratorPasses()
    {



        yield return new WaitForSeconds(0.25f);


        

    }
}
