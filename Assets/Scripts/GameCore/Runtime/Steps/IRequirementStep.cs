namespace GameCore
{
    public interface IRequirementStep
    {
        public string Description { get; }
        public void OnStepStart();
        public bool IsRequirementMet();
        public void OnStepComplete();
    }
}