using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColeSchwinghammer_HookShot_Script : MonoBehaviour
{
    LineRenderer line_renderer;
    Transform player_transform;
    [SerializeField] LayerMask HookShot_Wall;
    [SerializeField] float hookshot_speed = 10f;
    [SerializeField] float hookshot_launch_speed = 20f;
    [SerializeField] float hookshot_distance = 10f;

    bool hit_object = false;
    [HideInInspector] public bool is_returning = false;

    Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        line_renderer = GetComponent<LineRenderer>();
        player_transform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hit_object)
        {
            Vector2 hookshot_direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, hookshot_direction, hookshot_distance, HookShot_Wall);

            if (raycastHit.collider != null)
            {
                hit_object = true;
                target = raycastHit.point;
                line_renderer.enabled = true;
                line_renderer.positionCount = 2;

                StartCoroutine(HookShot());
            }
        }
        if (is_returning)
        {
            Vector2 HookShotPosition = Vector2.Lerp(transform.position, target, hookshot_speed * Time.deltaTime);

            transform.parent.position = HookShotPosition;

            line_renderer.SetPosition(0, transform.position);

            if(Vector2.Distance(transform.position, target) < 1.0f)
            {
                is_returning = false;
                hit_object = false;
                line_renderer.enabled = false;
            }
        }
    }

    IEnumerator HookShot()
    {
        float time = 10;

        line_renderer.SetPosition(0, transform.position);
        line_renderer.SetPosition(1, transform.position);

        Vector2 newPlayerPosition;

        for (float i = 0.0f; i < time; i += hookshot_launch_speed * Time.deltaTime)
        {
            newPlayerPosition = Vector2.Lerp(transform.position, target, i / time);
            line_renderer.SetPosition(0, transform.position);
            line_renderer.SetPosition(1, newPlayerPosition);
            yield return null;
        }

        line_renderer.SetPosition(1, target);
        
        is_returning = true;
    }
}
