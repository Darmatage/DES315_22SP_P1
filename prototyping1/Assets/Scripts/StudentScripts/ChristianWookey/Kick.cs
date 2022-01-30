using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{
    public GameObject KickInstance;
    public Animator PlayerAnimator;
    public SpriteRenderer PlayerSprite;

    private bool InCombo = false;
    private float KickPower = 0f;
    private float KickComboTimer = 0f;
    private float KickComboMin = 1f;
    private float KickComboMax = 0f;

    void Start()
    {
    }

    void Update()
    {
        // account for the player script flipping the player with the scale
        if (transform.parent.localScale.x < 0)
            transform.localScale = new Vector3(-1, 1);
        else
           transform.localScale = new Vector3(1, 1);

        if (InCombo)
        {
            if (KickComboTimer >= KickComboMin && KickComboTimer <= KickComboMax)
            {
                PlayerSprite.color = new Color(4f, 2f, 0f);
            }
            else
            {
                PlayerSprite.color = Color.white;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (InCombo)
            {
                if (KickComboTimer >= KickComboMin && KickComboTimer <= KickComboMax)
                {
                    KickPower = Mathf.Clamp(KickPower + 0.5f, 0f, 2.5f);
                }
                else
                {
                    InCombo = false;
                    KickPower = 0f;
                    PlayerSprite.color = Color.white;
                }
            }
            else
            {
                InCombo = true;
            }

            GameObject kickInst = GameObject.Instantiate(KickInstance, transform, false);

            Vector2 off = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
            float z = Mathf.Atan2(off.y, off.x) * Mathf.Rad2Deg + 180f;

            kickInst.transform.eulerAngles = new Vector3(0f, 0f, z);
            kickInst.transform.localPosition = new Vector3(0f, 0f, 0f);
            kickInst.GetComponent<KickWhoosh>().SetPower(KickPower + 0.5f);

            PlayerAnimator.SetTrigger("Kick");
            KickComboTimer = 0f;
            PlayerSprite.color = Color.white;
        }

        if (InCombo)
            KickComboTimer += Time.deltaTime;
    }

}
