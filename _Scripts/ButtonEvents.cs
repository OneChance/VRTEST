using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonEvents : MonoBehaviour {

	private Button button;
		
	void Awake(){
		button = GetComponent<Button>();
		button.onClick.AddListener(()=>Application.Quit());
	}
}
