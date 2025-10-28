using UnityEngine;

public class BundObjectivesCheckpoints : MonoBehaviour
{
    [SerializeField] StepManager _stepManagerScript;
    [SerializeField] LineRendererRope _lineRendererScript;
    private int currentStepIs;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        currentStepIs = _stepManagerScript.currentStepBundMiniGame;
        if (other.CompareTag("Player"))
        {
            switch (currentStepIs) // There's definitely a better way to do this but here we go
            {
                case 1:
                    if (_lineRendererScript.IsRopePickedUp() == true)
                    {
                        _stepManagerScript.StepCompleted(1);
                    }
                    break;

                case 2:

                    break;

            }
        }
    }
}
