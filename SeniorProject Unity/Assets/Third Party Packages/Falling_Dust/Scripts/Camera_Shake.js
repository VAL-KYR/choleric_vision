var CameraShake : GameObject;

function Start () {

}

function Update () {

    if (Input.GetButtonDown("Fire1"))
    {

        ShakeTheCamera();
       
    }
   
}

   
function ShakeTheCamera()
{

    CameraShake.GetComponent.<Animation>().Play();

}


