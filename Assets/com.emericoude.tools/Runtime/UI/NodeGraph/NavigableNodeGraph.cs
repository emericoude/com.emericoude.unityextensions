using System.Collections.Generic;
using UnityEngine;

namespace Emericoude.UI.NodeGraph
{
    public class NavigableNodeGraph : NodeGraph
    {
        public Node CurrentNode { get; private set; }
        public Node PreviousNode { get; private set; }
        [SerializeField] private Node m_StartingNode;
        
        private void Start() {
            if (this.m_StartingNode != null) this.TryNavigateTo(m_StartingNode);
        }
        
        public bool TryNavigateTo(Node node) {
            if (this.CurrentNode == null) {
                this.CurrentNode = node;
                return true;
            }

            if (!this.ConnectionsOut[this.CurrentNode].Contains(node)) return false;
            this.PreviousNode = this.CurrentNode;
            this.CurrentNode = node;
            return true;
        }

        public List<Node> GetPossibleNavigations()
        {
            if (this.CurrentNode == null) return null;
            if (!this.ConnectionsOut.ContainsKey(this.CurrentNode)) return null;
            return this.ConnectionsOut[this.CurrentNode];
        }
    }
}