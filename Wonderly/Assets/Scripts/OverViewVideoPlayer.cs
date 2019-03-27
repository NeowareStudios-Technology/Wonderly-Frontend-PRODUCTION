using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class OverViewVideoPlayer : MonoBehaviour {

	public VideoPlayer videoPlayer;
    private GameObject maincam;
    private AudioSource audioSource;
    public GameObject mainCanvas;
    public GameObject welcomePanel;
    public GameObject volumeOn;
    public GameObject volumeOff;

    void Start()
    {
        // Will attach a VideoPlayer to the main camera.
        audioSource = videoPlayer.gameObject.AddComponent<AudioSource>();

        // VideoPlayer automatically targets the camera backplane when it is added
        // to a camera object, no need to change videoPlayer.targetCamera.
        //videoPlayer = maincam.GetComponent<VideoPlayer>();

        // Play on awake defaults to true. Set it to false to avoid the url set
        // below to auto-start playback since we're in Start().
        //videoPlayer.playOnAwake = false;

        // By default, VideoPlayers added to a camera will use the far plane.
        // Let's target the near plane instead.
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

        // This will cause our Scene to be visible through the video being played.
        videoPlayer.targetCameraAlpha = 1.0F;

        // Set the video to play. URL supports local absolute or relative paths.
        // Here, using absolute.
        //videoPlayer.url = "/Users/graham/movie.mov";

        // Skip the first 100 frames.
        //videoPlayer.frame = 100;

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        // Each time we reach the end, we slow down the playback by a factor of 10.
        videoPlayer.loopPointReached += EndReached;

        // Start playback. This means the VideoPlayer may have to prepare (reserve
        // resources, pre-load a few frames, etc.). To better control the delays
        // associated with this preparation one can use videoPlayer.Prepare() along with
        // its prepareCompleted event.
        //videoPlayer.Play();
		
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        mainCanvas.GetComponent<PanelController>().OpenPanel(welcomePanel.GetComponent<Animator>());
    }
	public void playVideo(){
		SetCameraDepth(2);
        if (audioSource.mute){
            volumeOn.SetActive(true);
            volumeOff.SetActive(false);
            audioSource.mute = false;
        }

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);
        
        videoPlayer.Play();
        //Debug.Log("this shhould play");
	}
	public void stopVideo(){
        videoPlayer.Stop();
        SetCameraDepth(-1);
    }
    public void muteVideo(){
        if (audioSource.mute){
            volumeOn.SetActive(true);
            volumeOff.SetActive(false);
            audioSource.mute = false;
        }
        else{
            volumeOn.SetActive(false);
            volumeOff.SetActive(true);
            audioSource.mute = true;
        }     
    }
    private void SetCameraDepth(int cameraDepth){
        videoPlayer.gameObject.GetComponent<Camera>().depth = cameraDepth;
    }
}
