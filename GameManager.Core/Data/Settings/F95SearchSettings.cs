namespace GameManager.Core.Data.Settings
{
    public class F95SearchSettings
    {
        public List<F95Tag> DefaultF95Tags { get; set; } = new();
        public List<F95GamePrefix> DefaultF95Prefixes { get; set; } = new();
    }
}