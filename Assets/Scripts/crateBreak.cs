using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crateBreak : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> AffectedObjects;
    private Vector3 ForceVector;

    void Start()
    {
        StartCoroutine("addForceAndRemove");
    }

    private IEnumerator addForceAndRemove()
    {
        new WaitForSeconds(1);
        for (int I = 0; I < AffectedObjects.Count; I++)
        {
            ForceVector = new Vector3(Random.Range(-100,100), Random.Range(-50, 50), Random.Range(-20, 20));
            AffectedObjects[I].GetComponent<Rigidbody>().AddForce(ForceVector);
        }

        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
}
