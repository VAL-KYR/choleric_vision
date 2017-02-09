#pragma strict

var DustParticles : GameObject;
var LightBulbAnim : GameObject;
// var CameraShakeAnim : AnimationClip;
var CameraShake : GameObject;
var ExplosionAudio : AudioSource;
var BulbLight : Light;
var LightBulb : GameObject;
var LightBulbOff : GameObject;

private var BulbIntensity = 3.5;
private var RocketHitting = 0;


function Start () {

    DustParticles.SetActive(false);
    LightBulbOff.SetActive(false);
    // LightBulbAnim.SetActive(false);
    // print ("start!");
}

function Update () {

    if (Input.GetButtonDown("Fire1"))
    {

        RocketStrike();
       
    }

    BulbLight.intensity = BulbIntensity;

    if (RocketHitting == 0)
    {
        BulbIntensity = (Random.Range(3.4, 3.65));
    }

    if (RocketHitting == 1)
    {
        BulbIntensity = (Random.Range(2.0, 4.0));
    }
    
    if (RocketHitting == 2)
    {
        BulbIntensity = (Random.Range(3.0, 4.0));
    }
    
    
}

   
function RocketStrike()
{
    
    ExplosionAudio.Play();

    yield WaitForSeconds (1.5);
    




    
    DustParticles.SetActive(false);
    // LightBulbAnim.SetActive(false);
    DustParticles.SetActive(true); 
    // LightBulbAnim.SetActive(true);


    RocketHitting = 1;
    // BulbLight.intensity = (Random.Range(3.4, 3.65));


    DustParticles.SetActive(false);
    // LightBulbAnim.SetActive(false);
    DustParticles.SetActive(true); 
    // LightBulbAnim.SetActive(true);

    LightBulbAnim.GetComponent.<Animation>().Play();
    CameraShake.GetComponent.<Animation>().Play();

    // CameraShake.Play;

    LightBulb.SetActive(false);
    LightBulbOff.SetActive(true);
    yield WaitForSeconds (0.4);
    LightBulb.SetActive(true);
    LightBulbOff.SetActive(false);
    yield WaitForSeconds (0.4);
    LightBulb.SetActive(false);
    LightBulbOff.SetActive(true);
    yield WaitForSeconds (0.2);

    RocketHitting = 2;

    LightBulb.SetActive(true);
    LightBulbOff.SetActive(false);
    yield WaitForSeconds (0.2);
    LightBulb.SetActive(false);
    LightBulbOff.SetActive(true);
    yield WaitForSeconds (0.1);
    LightBulb.SetActive(true);
    LightBulbOff.SetActive(false);

    yield WaitForSeconds (1);
    RocketHitting = 0;

}

function FlickerBulb()
{



}



