using UnityEngine;
using UnityEngine.InputSystem;

public class PowerTest : MonoBehaviour
{
    public Vector3 pointerPos;
    [SerializeField] Camera cam;
    [SerializeField] float raycastDistance = 1000;
    InputAction powerInput;
    [SerializeField] GameObject blastCollider;

    [SerializeField] float blastCooldown = 0.5f;
    float blastTimer = 0;
    bool canBlast = true;

    void Start()
    {
        powerInput = InputSystem.actions.FindAction("BlastPower");
    }

    void Update()
    {
        if (!canBlast)
        {
            blastTimer += Time.deltaTime;
            if (blastTimer > blastCooldown)
            {
                blastTimer = 0;
                canBlast = true;
            }
        }


        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, raycastDistance))
        {
            pointerPos = hitData.point;
        }

        if (powerInput.WasPressedThisFrame() && canBlast)
        {
            LaunchBlast(pointerPos);
            canBlast = false;
        }
    }

    void LaunchBlast(Vector3 posParam)
    {
        Vector3 targetDirection = posParam - transform.position;

        GameObject spawnedBlast = Instantiate(blastCollider, transform.position, Quaternion.identity);
        Blast blastScript = spawnedBlast.GetComponent<Blast>();
        blastScript.FlyInDirection(targetDirection, posParam);
        Destroy(spawnedBlast, 5f);
    }
}
