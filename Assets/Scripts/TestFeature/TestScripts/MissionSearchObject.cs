using UnityEngine;

namespace c4g // need it but I don't know why - Lcc
{
    public class MissionSearchObject : TriggeredSearchedObject
    {
        // Function activates when the player uses the search action

        private bool wasSearched = false;

        private ObjectifManager _objectifManager;
        private Player _playerScript;

        [SerializeField] Material _objectiveColorMatTemp;


        private void Start()
        {
            _objectifManager = GameObject.Find("Player").GetComponent<ObjectifManager>(); // Code will not be reusable if each mission has its own objectif manager
            _playerScript = GameObject.Find("Player").GetComponent<Player>();
        }
        public override void Searched()
        {
            if (wasSearched == false)
            {
                wasSearched = true;
                _objectifManager.ObjectiveSearchedObjectFound(this.gameObject); // sends the current object to the list of found objects for the mission 
                Debug.Log("Found a mission search object");

                Renderer gameObjectColor = GetComponent<Renderer>();

                gameObjectColor.material = _objectiveColorMatTemp;
            }
        }
        public override void Talked() // Temp Test
        {
            Debug.Log("Hello, I am a npc");
            
                //_playerScript.UpdateCameraTEMP(gameObject);
            
            
        }
    }
}
