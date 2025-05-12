using System;
using UnityEngine;

namespace Emericoude.UI.NodeGraph
{
    //TODO: move this into Node, or make them a non-monoBehaviour
    [Obsolete("Should move this into the nodes themselves...")]
    public class NodeConnection : MonoBehaviour
    {
        public delegate void NodeChangeEvent(Node oldNode, Node newNode);
        public NodeChangeEvent OnToChanged { get; set; }
        public NodeChangeEvent OnFromChanged { get; set; }
        
        [SerializeField] protected Node m_From;
        public Node From {
            get => this.m_From;
            set {
                if (this.m_From == value) return;
                this.OnFromChanged.Invoke(this.m_From, value);
                this.m_From = value;
            }
        }
        
        [SerializeField] protected Node m_To;
        public Node To {
            get => this.m_To;
            set {
                if (this.m_To == value) return;
                this.OnToChanged.Invoke(this.m_To, value);
                this.m_To = value;
            }
        }
        
        [SerializeField] protected bool m_IsBidirectional;
        public bool Bidirectional { get => this.m_IsBidirectional; set => this.m_IsBidirectional = value; }
        
        
    }
}