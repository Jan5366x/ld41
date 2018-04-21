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

    public void interact()
    {
        var hits = Physics2D.CircleCastAll(new Vector2(transform.position.x, transform.position.y), 0.5f,
            new Vector2(0, 0));

        int minIdx = -1;
        float minDist = 99999;
        int idx = 0;
        Interactable bestInteractable = null;
        foreach (var hit in hits)
        {
            var collider = hit.collider;
            var interactable = collider.GetComponentInChildren<Interactable>();
            float dist = (collider.transform.position - this.transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                minIdx = idx;
                bestInteractable = interactable;
            }

            idx++;
        }

        if (minIdx >= 0 && bestInteractable != null)
        {
            if (bestInteractable.CanInteract(this))
            {
                bestInteractable.interact(this);
            }
        }
    }

    public void attackA()
    {
        throw new System.NotImplementedException();
    }

    public void attackB()
    {
        throw new System.NotImplementedException();
    }
}