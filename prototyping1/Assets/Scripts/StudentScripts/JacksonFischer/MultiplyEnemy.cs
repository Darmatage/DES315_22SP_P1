using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyEnemy : MonoBehaviour
{
    public int health = 1;
    public int multiplications = 2;
    public float time_between_multiplications = 5.0f;
    public GameObject enemy_object = null;

    private float dt = 0.0f;
    private Transform parent_transform;
    private int number_of_multiplications = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (number_of_multiplications < multiplications)
        {
            dt += UnityEngine.Time.deltaTime;

            if (dt >= time_between_multiplications)
            {
                dt = 0.0f;
                parent_transform = this.transform;
                SpawnChild(parent_transform);
                number_of_multiplications++;

            }
        }
    }

    void SpawnChild(Transform transform)
    {
        GameObject child_object = Instantiate(enemy_object);
        //GameObject child_object = enemy_object;
        int child_multiplications = multiplications / 2;

        MultiplyEnemy child_multiply_info = (MultiplyEnemy)child_object.GetComponent(typeof(MultiplyEnemy));
        child_multiply_info.health = health / 2;
        child_multiply_info.multiplications = child_multiplications;
        child_multiply_info.enemy_object = enemy_object;
        child_multiply_info.number_of_multiplications = 0;


        if (child_multiply_info.health < 1)
            child_multiply_info.health = 1;

         //child_object.AddComponent(typeof(MultiplyEnemy));
        //child_object->MultiplyEnemy = child_multiply_info;
        //Instantiate(child_object);
    }
}
