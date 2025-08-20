using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerVitals : MonoBehaviour
{
    [Header("Vitals Settings")]
    public Slider healthSlider;
    public int maxHealth;
    public float healthFallRate;

    public Slider thirstSlider;
    public int maxThirst;
    public float thirstFallRate;

    public Slider hungerSlider;
    public int maxHunger;
    public float hungerFallRate;

    public Slider staminaSlider;
    public int maxStamina;
    public float staminaFallRate;
    public int staminaFallMult;
    public float staminaRegainRate;
    public int staminaRegainMult;

    [Header("Temperature Setting")]
    public float frezzingTemp;
    public float CurrntTemp;
    public float normalTemp;
    public float heatTemp;
    public TextMeshProUGUI tempNumber;
    public TextMeshProUGUI cNumber;
    public Image tempBG;

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;

    [Header("XR Input")]
    public InputActionReference sprintAction;
    public InputActionReference moveAction;

    private CharacterController controller;
    private float currentSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        thirstSlider.maxValue = maxThirst;
        thirstSlider.value = maxThirst;

        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = maxHunger;

        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }

    void UpdateTemp()
    {
        tempNumber.text = CurrntTemp.ToString("00.0");
    }
    void Update()
    {
        if (CurrntTemp <= frezzingTemp)
        {
            tempBG.color = Color.blue;
            cNumber.color = Color.blue;
            UpdateTemp();
        }
        else if (CurrntTemp > heatTemp)
        {
            tempBG.color = Color.red;
            cNumber.color = Color.red;
            UpdateTemp();
        }
        else
        {
            tempBG.color = Color.green;
            cNumber.color = Color.green;
            UpdateTemp();
        }

        HandleVitals();
        HandleStamina();
        HandleMovement();
    }

    void HandleVitals()
    {
        hungerSlider.value = Mathf.Max(0, hungerSlider.value - Time.deltaTime / hungerFallRate);
        thirstSlider.value = Mathf.Max(0, thirstSlider.value - Time.deltaTime / thirstFallRate);

        if (hungerSlider.value <= 0 && thirstSlider.value <= 0)
        {
            healthSlider.value -= Time.deltaTime / healthFallRate * 2;
        }
        else if (hungerSlider.value <= 0 || thirstSlider.value <= 0 || CurrntTemp <= frezzingTemp || CurrntTemp >= heatTemp)
        {
            healthSlider.value -= Time.deltaTime / healthFallRate;
        }

        healthSlider.value = Mathf.Max(0, healthSlider.value);
        if (healthSlider.value <= 0)
        {
            CharacterDeath();
        }
    }

    void HandleStamina()
    {
        bool isSprinting = sprintAction.action.IsPressed();
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude > 0.1f;

        if (isMoving && isSprinting && staminaSlider.value > 0)
        {
            staminaSlider.value -= Time.deltaTime / staminaFallRate * staminaFallMult;
            if (staminaSlider.value > 0)
            {
                CurrntTemp += Time.deltaTime / 5;
            }
        }
        else
        {
            staminaSlider.value += Time.deltaTime / staminaRegainRate * staminaRegainMult;
            if (CurrntTemp >= normalTemp)
            {
                CurrntTemp -= Time.deltaTime / 10;
            }
        }

        staminaSlider.value = Mathf.Clamp(staminaSlider.value, 0, maxStamina);
    }

    void HandleMovement()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        bool isSprinting = sprintAction.action.IsPressed();
        currentSpeed = (isSprinting && staminaSlider.value > 0) ? sprintSpeed : walkSpeed;

        controller.Move(transform.TransformDirection(move) * currentSpeed * Time.deltaTime);
    }

    void CharacterDeath()
    {
        Debug.Log("Player has died.");
        // Optionally disable movement, inputs, etc.
    }

    public void RestoreHunger(float amount)
    {
        hungerSlider.value = Mathf.Min(hungerSlider.value + amount, maxHunger);
        Debug.Log($"🍓 Hunger restored by {amount}. Current hunger: {hungerSlider.value}");
    }

    public void RestoreThirst(float amount)
    {
        thirstSlider.value = Mathf.Min(thirstSlider.value + amount, maxThirst);
        Debug.Log($"🥤 Drank water: +{amount} thirst.");
    }
}