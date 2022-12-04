using k8s.Models;
using System;

namespace Kubmill.Models.Kubernetes
{
    public class K8Event
    {
        public DateTime? Timestamp { get; }
        public string? Reason { get; }
        public string? Message { get; }
        public bool IsError => Reason == "Failed";

        public K8Event(Corev1Event ev)
        {
            Message = ev.Message;
            Reason = ev.Reason;
            Timestamp = ev.LastTimestamp ?? ev.EventTime;
        }
    }
}
