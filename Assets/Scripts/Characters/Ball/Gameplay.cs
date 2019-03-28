using Profuder.Games.Characters.Ball;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BallController))]
public class Gameplay : MonoBehaviour {
    [SerializeField]
    private Text InstructionsText;

    private GameObject[] PickUps;

    private Int32 Count;

	void Start ()
    {
        PickUps = GameObject.FindGameObjectsWithTag("Pick Up");
        Count = 0;
    }

	void Update ()
    {
        SetInstruction();
	}

    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.CompareTag("Pick Up"))
        {
            Other.gameObject.SetActive(false);
            Count++;
        }
    }

    private void SetInstruction()
    {
        if (Count != PickUps.Length) InstructionsText.text = "Count: " + Count + "/" + PickUps.Length;
        else { InstructionsText.text = "You Win!"; GameObject.FindGameObjectWithTag("Door").SetActive(false); }
    }
}
