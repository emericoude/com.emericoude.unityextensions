using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emericoude.UI.NodeGraph
{

    public class NodeGraph : MonoBehaviour
    {
        [SerializeField] private Transform m_NodeContainer;
        [SerializeField] private Transform m_ConnectionContainer;

        [SerializeField] private Node m_StartingNode;
        public Node StartingNode {
            get => this.m_StartingNode;
            set => this.m_StartingNode = value;
        }

        [SerializeField] private bool m_GrabNodesFromHierarchyOnStart = true;

        private Node m_CurrentNode;
        public Node CurrentNode {
            get => this.m_CurrentNode;
            set {
                if (this.m_CurrentNode == value) return;
                this.m_CurrentNode = value;
                this.CacheCurrentNodeConnections();
            }
        }
        
        public List<NodeConnection> CurrentConnectionsIn { get; private set; }
        public List<NodeConnection> CurrentConnectionsOut { get; private set; }
        
        public List<Node> Nodes { get; private set; } = new();

        private void Start() {
            if (this.m_GrabNodesFromHierarchyOnStart) {
                var childrenNode = this.GetComponentsInChildren<Node>();
                foreach (var node in childrenNode) {
                    this.RegisterNode(node);
                }
            }
            
            this.CurrentNode = this.StartingNode;
        }

        public void RegisterNode(Node node) {
            if (this.m_NodeContainer != null) {
                node.transform.SetParent(this.m_NodeContainer);
            }

            if (this.m_ConnectionContainer != null) {
                foreach (var connection in node.Connections) {
                    connection.transform.SetParent(this.m_ConnectionContainer);
                }
            }

            node.Graph = this;
            node.OnConnectionAdded += this.OnNodeConnectionAdded;
            this.Nodes.Add(node);
        }
        
        public void DeregisterNode(Node node) {
            node.Graph = null;
            node.OnConnectionAdded -= this.OnNodeConnectionAdded;
            this.Nodes.Remove(node);
        }

        private void CacheCurrentNodeConnections() {
            if (this.CurrentNode == null) {
                this.CurrentConnectionsIn = null;
                this.CurrentConnectionsOut = null;
                return;
            }
            
            this.CurrentConnectionsIn = this.CurrentNode.GetConnectionsIn();
            this.CurrentConnectionsOut = this.CurrentNode.GetConnectionsOut();
        }

        private void OnNodeConnectionAdded(NodeConnection newConnection) {
            if (newConnection.From == this.CurrentNode || newConnection.To == this.CurrentNode) {
                this.CacheCurrentNodeConnections();
            }
            
            if (this.m_ConnectionContainer != null) {
                newConnection.transform.SetParent(this.m_ConnectionContainer);
            }
        }
        
        private void OnNodeConnectionRemoved(NodeConnection removedConnection) {
            if (removedConnection.From == this.CurrentNode || removedConnection.To == this.CurrentNode) {
                this.CacheCurrentNodeConnections();
            }
        }
    }
}
