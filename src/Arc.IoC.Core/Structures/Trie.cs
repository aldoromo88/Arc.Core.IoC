using System.Collections.Generic;
using Arc.Core.IoC.Contracts;

namespace Arc.Core.IoC.Structures
{
    internal class Trie : ITrie<ContainerRegistry>
    {
        #region Data Members

        /// <summary>
        /// Root TrieNode.
        /// </summary>
        private TrieNode rootTrieNode { get; set; }

        #endregion

        #region Ctors

        /// <summary>
        /// Creates a new Trie instance.
        /// </summary>
        internal Trie()
        {
            rootTrieNode = new TrieNode(' ');
        }

        #endregion


        #region Private methods

        public void Add(string name, ContainerRegistry element)
        {
            TrieNode trieNode = rootTrieNode;

            foreach (var c in name)
            {
                trieNode = trieNode.AddChild(c);
            }

            if (!trieNode.Registries.Contains(element))
            {
                trieNode.Registries.Add(element);
            }
        }

        public ISet<ContainerRegistry> Get(string name)
        {

            int searchCount = 0;
            var trieNode = rootTrieNode;
            foreach (var c in name)
            {
                var tempNode = trieNode.GetChild(c);

                if (tempNode == null)
                {
                    break;
                }

                trieNode = tempNode;
                searchCount++;
            }

            return name.Length == searchCount
                ? trieNode.Registries
                : new HashSet<ContainerRegistry>();
        }
    }
    #endregion
}
