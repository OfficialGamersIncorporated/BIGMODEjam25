using UnityEngine;
using UnityEngine.InputSystem;

public class PowerTest : MonoBehaviour
{
    
    public Vector3 pointerPos;
    [SerializeField] Camera cam;
    [SerializeField] float raycastDistance = 1000;
    [SerializeField] LayerMask ignoreLayerMask;



    // BLAST
    InputAction blastInput;
    [SerializeField] GameObject blastCollider;
    [SerializeField] float flySpeed = 50f;
    [SerializeField] float blastForce = 5f;
    [SerializeField] float blastCooldown = 0.5f;
    float blastTimer = 0;
    bool canBlast = true;


    // WALL
    InputAction wallInput;
    [SerializeField] GameObject wallCollider;
    [SerializeField] float riseSpeed = 5;
    [SerializeField] float wallForce = 5;
    [SerializeField] float wallCooldown = 0.5f;
    [SerializeField] float wallHeight = 5;
    [SerializeField] float wallLifeSpan = 1;
    float wallTimer = 0;
    bool canWall = true;

    void Start()
    {
        blastInput = InputSystem.actions.FindAction("BlastPower");
        wallInput = InputSystem.actions.FindAction("WallPower");
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


        if (!canWall)
        {
            wallTimer += Time.deltaTime;
            if (wallTimer > wallCooldown)
            {
                wallTimer = 0;
                canWall = true;
            }
        }


        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitData, raycastDistance, ~ignoreLayerMask))
        {
            pointerPos = hitData.point;
        }

        if (blastInput.WasPressedThisFrame() && canBlast)
        {
            LaunchBlast(pointerPos, flySpeed, blastForce);
            canBlast = false;
        }
        else if (wallInput.WasPressedThisFrame() && canWall)
        {
            RaiseWall(pointerPos, riseSpeed, wallForce, wallHeight, wallLifeSpan);
            canWall = false;
        }
    }

    void LaunchBlast(Vector3 posParam, float flySpeedParam, float blastForceParam)
    {
        Vector3 targetDirection = posParam - transform.position;

        GameObject spawnedBlast = Instantiate(blastCollider, transform.position, Quaternion.identity);
        Blast blastScript = spawnedBlast.GetComponent<Blast>();
        blastScript.FlyInDirection(targetDirection, posParam, flySpeedParam, blastForceParam);
        Destroy(spawnedBlast, 5f);
    }

    void RaiseWall(Vector3 posParam, float raiseSpeedParam, float wallForceParam, float wallHeightParam, float lifeSpanParam)
    {
        float startingHeight = posParam.y;
        float spawnHeight = (posParam.y - (wallHeightParam / 2) - 0.01f);

        Vector3 targetDirection = posParam - transform.position;

        GameObject spawnedWall = Instantiate(wallCollider, new Vector3(posParam.x, posParam.y - (wallHeightParam / 2) - 0.01f, posParam.z), Quaternion.identity);

        Quaternion rotation = Quaternion.LookRotation(new Vector3(spawnedWall.transform.position.x, transform.position.y, spawnedWall.transform.position.z) - transform.position, Vector3.up);
        spawnedWall.transform.rotation = rotation;

        Wall wallScript = spawnedWall.GetComponent<Wall>();
        wallScript.RaiseWall(startingHeight, spawnHeight, raiseSpeedParam, wallForceParam, wallHeightParam, lifeSpanParam);
    }
}
