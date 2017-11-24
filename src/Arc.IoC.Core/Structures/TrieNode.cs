using System;
using System.Collections.Generic;
using System.Diagnostics;
using Arc.Core.IoC.Contracts;

namespace Arc.Core.IoC.Structures
{
    [DebuggerDisplay("Character = {Character}")]
    internal class TrieNode : ITrieNode<ContainerRegistry>
    {

        #region Properties
        private IDictionary<char, TrieNode> Children { get; }
        private char Character { get; }
        public ISet<ContainerRegistry> Registries { get; }
        #endregion

        #region Constructors
        public TrieNode(char character)
        {
            Character = character;
            Children = new Dictionary<char, TrieNode>();
            Registries = new HashSet<ContainerRegistry>(new ContainerRegistryComparer());
        }
        #endregion

        internal TrieNode GetChild(char character)
        {
            TrieNode trieNode;
            Children.TryGetValue(character, out trieNode);
            return trieNode;
        }

        internal TrieNode AddChild(char c)
        {
            var child = GetChild(c);
            if (child != null) return child;

            child = new TrieNode(c);
            SetChild(child);
            return child;
        }

        private void SetChild(TrieNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }
            Children[child.Character] = child;
        }
    }
}
