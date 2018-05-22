using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour {

	public void Activate(List<Level> levels)
    {
        gameObject.SetActive(false);
    }
}
