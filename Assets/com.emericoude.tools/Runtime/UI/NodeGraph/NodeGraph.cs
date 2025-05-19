using System;
using System.Collections.Generic;

using UnityEngine;

using ZLinq;

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

        private List<Node> m_Nodes;
        public List<Node> Nodes {
            get {
                if (this.m_Nodes == null) this.CacheNodesAndConnections();
                return this.m_Nodes;
            }
        }

        private Dictionary<Node, List<Node>> m_ConnectionsOut;
        public Dictionary<Node, List<Node>> ConnectionsOut {
            get {
                if (this.m_ConnectionsOut == null) this.CacheNodesAndConnections();
                return this.m_ConnectionsOut;
            }
        }

        [SerializeField] private List<SerializedConnection> m_Connections;

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.cyan;
            if (Application.isPlaying) {
                foreach (var nodeConnections in this.ConnectionsOut) {
                    foreach (var connection in nodeConnections.Value) {
                        Debug.DrawLine(nodeConnections.Key.transform.position, connection.transform.position);
                    }
                }
            }
            else {
                foreach (var connection in this.m_Connections) {
                    Debug.DrawLine(connection.From.transform.position, connection.To.transform.position);
                }
            }
        }

        internal void CacheNodesAndConnections() {
            this.m_Nodes = new List<Node>();
            this.m_ConnectionsOut = new Dictionary<Node, List<Node>>();
            foreach (var connection in this.m_Connections) {
                if (connection.From == null) continue;
                if (connection.To == null) continue;
                if (!this.m_Nodes.Contains(connection.From)) this.m_Nodes.Add(connection.From);
                if (!this.m_Nodes.Contains(connection.To)) this.m_Nodes.Add(connection.To);
                if (!this.m_ConnectionsOut.ContainsKey(connection.To)) this.m_ConnectionsOut.Add(connection.To, new List<Node>());
                if (!this.m_ConnectionsOut.ContainsKey(connection.From)) this.m_ConnectionsOut.Add(connection.From, new List<Node>());
                this.m_ConnectionsOut[connection.From].Add(connection.To);
            }
        }

        public void AddConnection(Node from, Node to) {
            if (!this.m_Nodes.Contains(from)) this.m_Nodes.Add(from);
            if (!this.m_Nodes.Contains(to)) this.m_Nodes.Add(to);

            if (this.m_ConnectionsOut.TryGetValue(from, out var connectingTo)) {
                connectingTo.Add(to);
                return;
            }

            this.m_ConnectionsOut.Add(from, new List<Node>() { to });
        }

        public void RemoveConnection(Node from, Node to, bool removeNodesIfConnectionsAreEmpty = false) {
            if (!this.m_ConnectionsOut.TryGetValue(from, out var connectingTo)) return;
            connectingTo.Remove(to);

            if (!removeNodesIfConnectionsAreEmpty) return;
            if (connectingTo.Count == 0 && !this.m_ConnectionsOut.Values.AsValueEnumerable().Any(c => c.Contains(from))) {
                this.m_ConnectionsOut.Remove(from);
                this.m_Nodes.Remove(from);
            }

            if (this.m_ConnectionsOut[to].Count == 0 && !this.m_ConnectionsOut.Values.AsValueEnumerable().Any(c => c.Contains(from))) {
                this.m_ConnectionsOut.Remove(to);
                this.m_Nodes.Remove(to);
            }
        }
    }
}