//Author & Copyright: Charles Osberg
//Licensed under MIT, DES315 2022 edition.
//For updates, bug reports, and feature requests, see https://github.com/MCFX2/unity-multitag

using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, ExecuteAlways]
public class COMultitag : MonoBehaviour
{
    [SerializeField] private List<string> _tags = new List<string>();
    public List<string> Tags => _tags;

    private void OnEnable()
    {
        COTags.RegisterGameObjectTags(gameObject, Tags.ToArray());
    }

    private void OnDisable()
    {
        COTags.UnregisterGameObjectTags(gameObject, Tags.ToArray());
    }
}