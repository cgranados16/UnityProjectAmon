using UnityEngine;
using System.Collections;


[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]

public class CharacterMovementControl : MonoBehaviour
{

	public float animSpeed = 1.5f;		
	public float lookSmoother = 3.0f;
	public bool useCurves = true;

	public float useCurvesHeight = 0.5f;

	public float forwardSpeed = 30.0f;

	public float backwardSpeed = 20.0f;

	public float rotateSpeed = 2.0f;
	
	public float jumpPower = 3.0f; 
	
	private CapsuleCollider col;
	private Rigidbody rb;
	
	private Vector3 velocity;
	
	private float orgColHight;
	private Vector3 orgVectColCenter;
	
	private Animator anim;							
	private AnimatorStateInfo currentBaseState;		

	private GameObject cameraObject;	
		
	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int restState = Animator.StringToHash("Base Layer.Rest");


	void Start ()
	{
		
		anim = GetComponent<Animator>();
		
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();
		
		cameraObject = GameObject.FindWithTag("MainCamera");
		
		orgColHight = col.height;
		orgVectColCenter = col.center;
}
	
	

	void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal");		
		float v = Input.GetAxis("Vertical");		
		anim.SetFloat("Speed", v);					
		anim.SetFloat("Direction", h); 				
		anim.speed = animSpeed;						
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	
		rb.useGravity = true;
		
		velocity = new Vector3(0, 0, v);	
		velocity = transform.TransformDirection(velocity);
		
		if (v > 0.1) {
			velocity *= forwardSpeed;
		} else if (v < -0.1) {
			velocity *= backwardSpeed;
		}
		
		if (Input.GetButtonDown("Jump")) {


			if (currentBaseState.nameHash == locoState){
				if(!anim.IsInTransition(0))
				{
						rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool("Jump", true);		
				}
			}
		}
		

		transform.localPosition += velocity * Time.fixedDeltaTime;
		transform.Rotate(0, h * rotateSpeed, 0);	
	
		if (currentBaseState.nameHash == locoState){
			//カーブでコライダ調整をしている時は、念のためにリセットする
			if(useCurves){
				resetCollider();
			}
		}
		// JUMP中の処理
		// 現在のベースレイヤーがjumpStateの時
		else if(currentBaseState.nameHash == jumpState)
		{
			cameraObject.SendMessage("setCameraPositionJumpView");
			
			if(!anim.IsInTransition(0))
			{
				
				// 以下、カーブ調整をする場合の処理
				if(useCurves){
					// 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
					// JumpHeight:JUMP00でのジャンプの高さ（0〜1）
					// GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
					float jumpHeight = anim.GetFloat("JumpHeight");
					float gravityControl = anim.GetFloat("GravityControl"); 
					if(gravityControl > 0)
						rb.useGravity = false;	//ジャンプ中の重力の影響を切る
										
					// レイキャストをキャラクターのセンターから落とす
					Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
					RaycastHit hitInfo = new RaycastHit();
					// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
					if (Physics.Raycast(ray, out hitInfo))
					{
						if (hitInfo.distance > useCurvesHeight)
						{
							col.height = orgColHight - jumpHeight;			// 調整されたコライダーの高さ
							float adjCenterY = orgVectColCenter.y + jumpHeight;
							col.center = new Vector3(0, adjCenterY, 0);	// 調整されたコライダーのセンター
						}
						else{
							// 閾値よりも低い時には初期値に戻す（念のため）					
							resetCollider();
						}
					}
				}
				// Jump bool値をリセットする（ループしないようにする）				
				anim.SetBool("Jump", false);
			}
		}

		else if (currentBaseState.nameHash == idleState)
		{
			if(useCurves){
				resetCollider();
			}
			if (Input.GetButtonDown("Jump")) {
				anim.SetBool("Rest", true);
			}
		}
		else if (currentBaseState.nameHash == restState)
		{
			//cameraObject.SendMessage("setCameraPositionFrontView");
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Rest", false);
			}
		}
	}

	void resetCollider()
	{
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}

	void OnTriggerEnter(Collider obj){
    	//print ("Object Hit");
	}

}
