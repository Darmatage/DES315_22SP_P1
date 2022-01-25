using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyEnemy : MonoBehaviour
{
    public int health = 1;
    public int multiplications = 2;
    public float time_between_multiplications = 5.0f;
    public GameObject enemy_object = null;
    public GameObject enemy_art = null;


    private float dt = 0.0f;
    private Transform parent_transform;
    private int number_of_multiplications = 0;
    private Renderer enemy_renderer;
    private Color starting_color;

    // Start is called before the first frame update
    void Start()
    {
        enemy_renderer = (Renderer)enemy_art.GetComponent(typeof(Renderer));
        starting_color = enemy_renderer.material.GetColor("_Color");

    }

    // Update is called once per frame
    void Update()
    {
        Color current_color = enemy_renderer.material.GetColor("_Color");

        if (number_of_multiplications < multiplications)
        {
            //Color current_color = enemy_renderer.material.GetColor("_Color");
            float seconds_left = (time_between_multiplications - dt);

            dt += UnityEngine.Time.deltaTime;

            if (seconds_left <= 1.0f)
            {
                enemy_renderer.material.SetColor("_Color", Color.red);
            }

            else if (seconds_left > 1.0f || current_color != starting_color)
            {
                enemy_renderer.material.SetColor("_Color", starting_color);
            }

            if (dt >= time_between_multiplications)
            {
                dt = 0.0f;
                parent_transform = this.transform;
                SpawnChild(parent_transform);
                number_of_multiplications++;

            }
        }

        else if (number_of_multiplications == multiplications && current_color != starting_color)
            enemy_renderer.material.SetColor("_Color", starting_color);
    }

    void SpawnChild(Transform transform)
    {
        GameObject child_object = Instantiate(enemy_object);
        int child_multiplications = multiplications / 2;

        MultiplyEnemy child_multiply_info = (MultiplyEnemy)child_object.GetComponent(typeof(MultiplyEnemy));
        child_multiply_info.health = health / 2;
        child_multiply_info.multiplications = child_multiplications;
        child_multiply_info.enemy_object = enemy_object;
        child_multiply_info.number_of_multiplications = 0;


        if (child_multiply_info.health < 1)
            child_multiply_info.health = 1;

        
        Transform child_transform = (Transform) child_object.GetComponent(typeof(Transform));
        Transform this_transform = (Transform)this.gameObject.GetComponent(typeof(Transform));
        child_transform.localScale = this_transform.localScale / 1.5f;


        GameObject child_art = child_object.transform.GetChild(0).gameObject;
        Renderer child_renderer = (Renderer)child_art.GetComponent(typeof(Renderer));
        child_renderer.material.SetColor("_Color", starting_color);

    }
}
