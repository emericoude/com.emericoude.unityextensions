using System;
using System.Collections.Generic;
using UnityEngine;
using ZLinq;

namespace Emericoude.UI.NodeGraph
{
    [RequireComponent(typeof(RectTransform))]
    public class Node : MonoBehaviour
    {
        public Action<NodeConnection> OnConnectionAdded { get; set; }
        public Action<NodeConnection> OnConnectionRemoved { get; set; }
        public Action<Vector2> OnNodeMoved { get; set; }

        [SerializeField, HideInInspector] private RectTransform m_RectTransform;
        public RectTransform RectTransform {
            get {
                if (this.m_RectTransform == null) this.m_RectTransform = this.GetComponent<RectTransform>();
                return this.m_RectTransform;
            }
        }
        
        [SerializeField] private NodeGraph m_Graph;
        public NodeGraph Graph {
            get => this.m_Graph;
            set {
                if (this.m_Graph == value) return;
                NodeGraph oldGraph = this.m_Graph;
                this.m_Graph = value;
                if (oldGraph != null) oldGraph.DeregisterNode(this);
                if (this.m_Graph != null) value.RegisterNode(this);
            }
        }
        
        [SerializeField] private List<NodeConnection> m_Connections = new();
        public List<NodeConnection> Connections => this.m_Connections;
        
        public List<NodeConnection> GetConnectionsIn() => this.m_Connections.AsValueEnumerable().Where(c => c.Bidirectional || c.To == this).ToList();
        public List<NodeConnection> GetConnectionsOut() => this.m_Connections.AsValueEnumerable().Where(c => c.Bidirectional || c.From == this).ToList();

        private Vector3 position;

        private void OnEnable() {
            if (this.Graph != null) this.Graph.RegisterNode(this);
        }

        private void OnDisable() {
            if (this.Graph != null) this.Graph.DeregisterNode(this);
        }

        //TODO: we can probably do this via events instead
        private void Update() {
            if (this.position != this.transform.position) {
                this.position = this.transform.position;
                this.OnNodeMoved?.Invoke(this.position);
            }
        }
        
        public void AddConnection(NodeConnection connection) {
            this.m_Connections.Add(connection);
            this.OnConnectionAdded?.Invoke(connection);
        }

        public void RemoveConnection(NodeConnection connection) {
            if (!this.m_Connections.Contains(connection)) return;
            this.m_Connections.Remove(connection);
            this.OnConnectionRemoved?.Invoke(connection);
        }
    }
}