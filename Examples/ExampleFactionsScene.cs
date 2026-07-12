using Systems.SimpleCore.Operations;
using Systems.SimpleFactions.Abstract;
using Systems.SimpleFactions.Data;
using Systems.SimpleFactions.Utility;
using UnityEngine;

namespace Systems.SimpleFactions.Examples
{
    [DisallowMultipleComponent]
    public sealed class ExampleFactionsScene : MonoBehaviour
    {
        [SerializeField] private ExampleFactionMembership _membership;
        [SerializeField] private long _reputationGain = 125L;
        [SerializeField] private ReputationLevelBase _manualLevel;

        private void Awake()
        {
            if (!_membership && !TryGetComponent(out _membership))
                _membership = gameObject.AddComponent<ExampleFactionMembership>();

            if (!TryGetComponent(out ExampleFactionHolder holder))
                holder = gameObject.AddComponent<ExampleFactionHolder>();
        }

        private void Start()
        {
            RunExample();
        }

        [ContextMenu("Run Factions Example")]
        public void RunExample()
        {
            if (!_membership)
            {
                Debug.LogWarning("[SimpleFactions] Example membership component is not assigned.");
                return;
            }

            ExampleFaction faction = FactionDatabase.GetExact<ExampleFaction>();
            if (ReferenceEquals(faction, null))
            {
                Debug.LogWarning("[SimpleFactions] ExampleFaction was not found in the faction database. Let the auto-create/addressables setup generate it before running faction operations.");
                return;
            }

            OperationResult joinResult = FactionAPI.Join<ExampleFaction, ExampleFactionHolder>(_membership);
            OperationResult reputationResult = FactionAPI.ChangeReputation<ExampleFaction, ExampleFactionHolder>(
                _membership,
                _reputationGain);

            ReputationLevelBase currentLevel =
                FactionAPI.GetLevel<ExampleFaction, ExampleFactionHolder>(_membership);
            string currentLevelName = ReferenceEquals(currentLevel, null) ? "none" : currentLevel.name;

            Debug.Log("[SimpleFactions] Join result: " + joinResult +
                      ", reputation result: " + reputationResult +
                      ", reputation: " + _membership.GetReputation<ExampleFaction>() +
                      ", current level: " + currentLevelName);

            if (!_manualLevel) return;

            OperationResult levelResult =
                FactionAPI.AssignLevel<ExampleFaction, ExampleFactionHolder>(_membership, _manualLevel);
            Debug.Log("[SimpleFactions] Manual level assignment result: " + levelResult);
        }
    }
}
