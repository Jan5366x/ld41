using UnityEngine;

namespace World.Helper
{
    [ExecuteInEditMode]
    public class OrderScript : MonoBehaviour
    {
        public float offset;

        public void Awake()
        {
            SetPosition(); 
        }

        public void Update()
        {
            SetPosition();
        }

        public void SetPosition()
        {
            var renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = (int) -((transform.position.y + offset) * 100);
            }
        }
    }
}