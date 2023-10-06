namespace Engine.Assets
{
    public class Asset
    {
        public string Path { get; internal set; }

        public Asset(string path)
        {
            Path = path;
        }
    }

    public static class AssetManager
    {
        private static readonly Dictionary<string, Asset> _assets = new Dictionary<string, Asset>();

        public static T Get<T>(string path) where T : Asset, new()
        {
            if(_assets.ContainsKey(path))
            {
                return (T)_assets[path];
            }

            if (Activator.CreateInstance(typeof(T), path) is T asset)
            {
                _assets.Add(path, asset);
                return asset;
            }

            throw new NullReferenceException("Asset is null");
        }
    }
}
