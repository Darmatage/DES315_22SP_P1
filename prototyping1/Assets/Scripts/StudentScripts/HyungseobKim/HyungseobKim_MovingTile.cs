using UnityEngine;

public abstract class HyungseobKim_MovingTile : MonoBehaviour
{
    public HyungseobKim_Trigger trigger;
    public float amount;

    private bool moving = false;
    private float originalPos;

    private static float timeLimit = 1.0f;
    private float movedTime = 0.0f;

    // Start is called before the first frame update
    private void Start()
    {
        trigger.triggerEvent.AddListener(OnTrigger);
    }

    // Update is called once per frame
    private void Update()
    {
        if (moving)
        {
            movedTime += Time.deltaTime;

            if (movedTime > timeLimit)
            {
                movedTime = timeLimit;
                moving = false;
            }

            Move(originalPos + amount * movedTime);
        }
    }

    public void OnTrigger()
    {
        moving = true;

        originalPos = GetOriginalPos();
    }

    protected abstract void Move(float posToMove);
    protected abstract float GetOriginalPos();
}
