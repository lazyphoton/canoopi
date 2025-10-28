namespace GameCore
{
    public interface ILaunchParameters
    {
        public bool TryGetValue(string key, out string value);
    }
}