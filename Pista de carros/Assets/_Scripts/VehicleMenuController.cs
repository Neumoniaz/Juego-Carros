using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class VehicleMenuController : MonoBehaviour
{
    [Header("Componentes de UI y Video")]
    public GameObject menuPanel;
    public VideoPlayer mainVideoPlayer; 
    public TextMeshProUGUI carNameText;
    public Scrollbar speedScrollbar;
    public Scrollbar brakeScrollbar;
    public Scrollbar angleScrollbar;

    [Header("Cars")]
    public CameraController cam;
    public Transform initialPos;
    
    // Almacena los datos de los carros (ahora incluyen el video)
    public CarSo[] cars; 

    [SerializeField] private float maxScrollbar = 2000;
    [SerializeField] private float maxScrollbarAngle = 60;

    private int carIndex = 0;
    private CarSo selectedCar;

    private void Start()
    {
        UpdateMenuUI(0);
    }

    // Este método centraliza la actualización gráfica de todo el menú
    public void UpdateMenuUI(int index)
    {
        if (index < 0 || index >= cars.Length) return;

        carIndex = index;
        selectedCar = cars[carIndex];

        // 1. Actualizar el Video
        if (selectedCar.videoClip != null)
        {
            mainVideoPlayer.clip = selectedCar.videoClip;
            mainVideoPlayer.Play();
        }

        // 2. Actualizar Textos y Barras
        carNameText.text = selectedCar.carName;
        speedScrollbar.size = selectedCar.speed / maxScrollbar;
        brakeScrollbar.size = selectedCar.brakeForce / maxScrollbar;
        angleScrollbar.size = selectedCar.angle / maxScrollbarAngle;
    }

    public void NextVehicle()
    {
        int nextIndex = (carIndex + 1) % cars.Length;
        UpdateMenuUI(nextIndex);
    }

    public void PreviousVehicle()
    {
        int prevIndex = carIndex - 1;
        if (prevIndex < 0) prevIndex = cars.Length - 1;
        UpdateMenuUI(prevIndex);
    }

    public void SelectCar()
    {
        // 1. Instanciar el carro y asignar la cámara
        GameObject prefabSelected = Instantiate(selectedCar.carPrefab, initialPos.position, Quaternion.identity);
        cam.target = prefabSelected.transform;

        // 2. Detener el video para no consumir recursos en segundo plano
        if (mainVideoPlayer != null)
        {
            mainVideoPlayer.Stop();
        }

        // 3. Apagar la interfaz del menú para ver el juego
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }
}