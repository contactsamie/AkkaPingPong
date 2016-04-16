namespace AkkaPingPong.ActorSystemLib
{
    public class ActorMetaData
    {
        public ActorMetaData(string name, ActorMetaData parent = null)
        {
            Name = name;
            Parent = parent;
            // if no parent, we assume a top-level actor
            var parentPath = parent != null ? parent.Path : "";
            Path = Name.StartsWith("akka:") && string.IsNullOrEmpty(parentPath) ?
                string.Format("{0}", Name) :
                string.Format("{0}/{1}", parentPath, Name);
        }

        public string Name { get; }

        public ActorMetaData Parent { get; set; }

        public string Path { get; }
    }
}