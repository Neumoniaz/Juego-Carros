using UnityEngine;
using UnityEngine.Video;

public class SelectionMenu : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoClip vehicleVideo; // Asigna el video de este auto en el Inspector

    public void OnSelectVehicle()
    {
        if (videoPlayer != null && vehicleVideo != null)
        {
            videoPlayer.clip = vehicleVideo;
            videoPlayer.Play();
        }
    }
}