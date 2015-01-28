using UnityEngine;
using System.Collections;
using System;

public class PlayerCtrl : MonoBehaviour {

	private Animator _animator;
	private AnimatorStateInfo _currentStateInfo;
	private AnimatorStateInfo _preStateInfo;

	public float waitTime = 3f;
	public bool isRandom = true;

	public AnimationClip[] _faceClips;
	public string[] _faceMotionNames;

	public AudioClip[] _voiceClips;
	public AudioClip[] _hourClip;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		_preStateInfo = _currentStateInfo;

		//加载脸部动画片段
		_faceClips = Resources.LoadAll<AnimationClip>("anims");
		_voiceClips = Resources.LoadAll<AudioClip>("voice");
		_hourClip = Resources.LoadAll<AudioClip>("hourvoice");

		_faceMotionNames = new string[_faceClips.Length];
		for(int i=0;i<_faceClips.Length;i++){
			_faceMotionNames[i] = _faceClips[i].name;
		}

		StartCoroutine(RandomChangeMotion());
	}

	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit,Mathf.Infinity)){
				if(hit.collider.tag == "face"){
					ChangeFace();
				}
			}
		}


		if(_animator.GetBool("Next")){
			_currentStateInfo=_animator.GetCurrentAnimatorStateInfo(0);
			if(_preStateInfo.nameHash != _currentStateInfo.nameHash){
				_animator.SetBool("Next",false);
				_preStateInfo = _currentStateInfo;
			}
		}
	}

	private void ChangeFace(){
		_animator.SetLayerWeight(1,1);
		int index = UnityEngine.Random.Range(0,_faceMotionNames.Length-1);
		_animator.CrossFade(_faceMotionNames[index],0);
		if(audio.isPlaying){
			audio.Stop();
		}
		audio.clip = _voiceClips[index];
		audio.Play();
	}


	IEnumerator RandomChangeMotion(){
		while(true){
			if(isRandom){
				_animator.SetBool("Next",true);
			}
			yield return new WaitForSeconds(waitTime);
		}
	}

	public void OnAskTime(){
		int hour = DateTime.Now.Hour;
		if(audio.isPlaying){
			audio.Stop();
		}
		audio.clip = _hourClip[hour];
		audio.Play();
	}

}
