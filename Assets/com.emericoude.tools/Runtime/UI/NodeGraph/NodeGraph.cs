using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emericoude.UI.NodeGraph
{
    public class NodeGraph : MonoBehaviour
    {
        [Serializable]
        internal struct SerializedConnection
        {
            public Node From;
            public Node To;
        }
        
        public List<Node> Nodes { get; private set; }

        private Dictionary<Node, List<Node>> m_ConnectionsOut;
        public Dictionary<Node, List<Node>> ConnectionsOut {
            get {
                if (this.m_ConnectionsOut == null) this.CacheNodesAndConnections();
                return this.m_ConnectionsOut;
            }
        }

        [SerializeField] private List<SerializedConnection> m_Connections;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            if (Application.isPlaying)
            {
                foreach (var nodeConnections in this.ConnectionsOut)
                {
                    foreach (var connection in nodeConnections.Value)
                    {
                        Debug.DrawLine(nodeConnections.Key.transform.position, connection.transform.position);
                    }
                }
            }
            else
            {
                foreach (var connection in this.m_Connections)
                {
                    Debug.DrawLine(connection.From.transform.position, connection.To.transform.position);
                }
            }
        }

        internal void CacheNodesAndConnections()
        {
            this.Nodes = new List<Node>();
            this.m_ConnectionsOut = new Dictionary<Node, List<Node>>();
            foreach (var connection in this.m_Connections) {
                if (connection.From == null) continue;
                if (connection.To == null) continue;
                if (!this.Nodes.Contains(connection.From)) this.Nodes.Add(connection.From);
                if (!this.Nodes.Contains(connection.To)) this.Nodes.Add(connection.To);
                this.AddConnection(connection.From, connection.To);
            }
        }

        public void AddConnection(Node from, Node to) {
            if (this.ConnectionsOut.TryGetValue(from, out var connectingTo)) {
                connectingTo.Add(to);
                return;
            }
            this.ConnectionsOut.Add(from, new List<Node>() { to });
        }

        public void RemoveConnection(Node from, Node to)
        {
            if (!this.ConnectionsOut.TryGetValue(from, out var connectingTo)) return;
            connectingTo.Remove(to);
            if (connectingTo.Count == 0)
            {
                this.ConnectionsOut.Remove(from);
            }
        }
    }
}