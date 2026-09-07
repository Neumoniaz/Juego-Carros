using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Tipo de Piloto")]
    public bool esNPC = false; // Si marcas esto en el Inspector, el carro se maneja solo
    
    [HideInInspector] public float inputAceleracion;
    [HideInInspector] public float inputGiro;
    [Header("Modelos Visuales")]
    public GameObject modeloNormal;
    public GameObject modeloCastigado;
    // Para almacenar el Rigidbody
    private Rigidbody _rb;

    // Variable del Scriptable Object del carro.
    [Header("Values")]
    public CarSo car;

    // Variable para almacenar la velocidad final del carro
    [HideInInspector] public float speed;

    // Variable para almacenar el angulo del carro
    private float _steeringAngle;

    // Arreglo para almacenar los colisionadores de llanta
    [Header("Wheels")]
    [SerializeField] private WheelCollider[] _wheelCollider;

    // Arreglo para almacenar las llantas fisicas
    [SerializeField] private Transform[] _wheelTransform;

    // Luces
    //public GameObject lights; 

    // Start is called before the first frame update
    void Start()
    {

        // Se guardan el componente del Rigidbody en la variable
        _rb = GetComponent<Rigidbody>();

        // Cambia el centro de gravedad del carro
        _rb.centerOfMass = new Vector3(0, -1f, 0);

        // Guarda la velocidad inicial
        speed = car.speed;

    }

    private void FixedUpdate()
    {
        if (!esNPC)
        {
            inputAceleracion = InputController.instance.movementVector.y;
            inputGiro = InputController.instance.movementVector.x;
        }
        // Se llaman a los metodos respectivos en Fixed Update, ya que es el indicado para llamar metodos relacionados
        // con fisicas
        Motor();
        Brake();
        Steering();
        UpdateWheels();
    }

    // motorTorque le asigna velocidad a los colliders. Se aplica a cada uno de los 4 y se le multiplica 
    // Por la velocidad
    public void Motor()
    {
        foreach (var wheel in _wheelCollider)
        {
            wheel.motorTorque = inputAceleracion * speed;
        }
    }

    // Para frenar se toma el input del script InputController, el cual valida si el freno se presiona o no
    // Luego si se presiona se le aplica la fuerza de frenado con brakeTorque y si no esta queda en 0
    public void Brake()
    {
        if (InputController.instance.isBraking)
        {
            // Se recorre cada llanta en el grupo de los colisionadores y se le aplica la fuerza de freno
            foreach (var wheel in _wheelCollider)
            {
                wheel.brakeTorque = car.brakeForce;
            }
        }
        else
        {
            // Se recorre cada llanta en el grupo de los colisionadores y la fuerza de freno queda nuevamente en 
            // 
            foreach (var wheel in _wheelCollider)
            {
                wheel.brakeTorque = 0;
            }
            //lights.SetActive(false);
        }
    }

    //Método que aplica el giro en las ruedas. Solo se toman las dos delanteras
    public void Steering()
    {
        _steeringAngle = car.angle * inputGiro;
        _wheelCollider[2].steerAngle = _steeringAngle;
        _wheelCollider[3].steerAngle = _steeringAngle;
    }

    // Método que permite actualizar las ruedas con el movimiento del collider usando el metido creado abajo.
    public void UpdateWheels()
    {
        for (int i = 0; i < _wheelCollider.Length; i++)
        {
            UpdateSingleWheel(_wheelCollider[i], _wheelTransform[i]);
        }
    }

    // Metodo en donde se toma cada rueda con cada collider y se le asigna a la rueda la posicion y rotacion del collider
    public void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        // Se obtiene la posición y rotación del collider.
        wheelCollider.GetWorldPose(out pos, out rot);

        // Se aplican estos datos al transform.
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
    public System.Collections.IEnumerator ActivateBoost(float multiplier, float duration)
    {
        // 1. Aumentamos la fuerza del motor
        speed = car.speed * multiplier;
        
        // 2. Si es un Boost (multiplicador mayor a 1), le damos un empujón físico instantáneo
        if (multiplier > 1f)
        {
            // Fuerza de impulso hacia adelante. Puedes ajustar el número 50000 según el peso de tu carro
            _rb.AddForce(transform.forward * 50000f, ForceMode.Impulse);
        }
        // 3. Si es lentitud (multiplicador menor a 1), frenamos el carro reduciendo su velocidad física
        else if (multiplier < 1f)
        {
            _rb.linearVelocity = _rb.linearVelocity * 0.3f; // Corta la inercia de golpe
        }
        
        yield return new WaitForSeconds(duration);
        
        speed = car.speed;
    }
    public System.Collections.IEnumerator ActivateTrap(float speedMultiplier, float duration)
    {
        // Reducimos la velocidad multiplicando por un decimal (ej. 0.3)
        speed = car.speed * speedMultiplier;
        
        // Apagamos el modelo normal y prendemos el modelo lento
        if(modeloNormal != null && modeloCastigado != null)
        {
            modeloNormal.SetActive(false);
            modeloCastigado.SetActive(true);
        }
        
        // El script espera los segundos del castigo
        yield return new WaitForSeconds(duration);
        
        // Devolvemos la velocidad a la normalidad
        speed = car.speed;
        
        // Regresamos al modelo original
        if(modeloNormal != null && modeloCastigado != null)
        {
            modeloCastigado.SetActive(false);
            modeloNormal.SetActive(true);
        }
    }
}
