using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    public Unit Template;

    private GameObject _presentation;
    // Use this for initialization

    void Start()
    {
        // instantiate the presentation object
        _presentation = Instantiate(Template.Presentation, transform);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject Presentation
    {
        get { return _presentation; }
    }

    public void move(float dx, float dy, bool sprint)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        if (Mathf.Abs(dx) > Mathf.Abs(dy))
        {
            dy = 0;
        }
        else if (Mathf.Abs(dx) < Mathf.Abs(dy))
        {
            dx = 0;
        }

        dx *= Template.Acceleration;
        dy *= Template.Acceleration;
        if (sprint)
        {
            dx *= (1 + Random.value * 0.2f);
            dy *= (1 + Random.value * 0.2f);
        }

        body.AddForce(new Vector2(dx, dy));
    }
}