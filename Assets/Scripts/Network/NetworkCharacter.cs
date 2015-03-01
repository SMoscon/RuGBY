using UnityEngine;
 
public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;

    Animator animator;
    HashIDs hash;

    void Start()
    {
        animator = GetComponent<Animator>();
        hash = GetComponent<HashIDs>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }
 
void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
{
    if (stream.isWriting)
    {
        // We own this player: send the others our data
        stream.SendNext(transform.position);
        stream.SendNext(transform.rotation);
 
        stream.SendNext(animator.GetFloat(hash.speedFloat));
        stream.SendNext(animator.GetBool(hash.attackingBool));
        stream.SendNext(animator.GetBool(hash.smashingBool));
        stream.SendNext(animator.GetBool(hash.defendingBool));
        //Debug.Log ("Sending my position: " + transform.position + ", rotation: "+transform.rotation + ", current hash:" + (int)animator.GetCurrentAnimatorStateInfo(0).nameHash);
    }
    else
    {
        // Network player, receive data
        this.correctPlayerPos = (Vector3)stream.ReceiveNext();
        this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
 
        //PlayerController myC = GetComponent<PlayerController>();
        //myC.currentTagHash = (int)stream.ReceiveNext();
        animator.SetFloat(hash.speedFloat, (float)stream.ReceiveNext());
        animator.SetBool(hash.attackingBool, (bool)stream.ReceiveNext());
        animator.SetBool(hash.smashingBool, (bool)stream.ReceiveNext());
        animator.SetBool(hash.defendingBool, (bool)stream.ReceiveNext());
        //Debug.Log ("Receiving position: " + this.correctPlayerPos + ", rotation: "+this.correctPlayerRot + ", current hash:" + myC.currentTagHash);

    }
}
}