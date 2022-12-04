using k8s.Models;
using System;
using System.Linq;

namespace Kubmill.Models.Kubernetes.Extensions
{
    public static class PodExtensions
    {
        public static string? GetRestartStatus(this V1Pod pod)
        {
            var status = pod.Status.ContainerStatuses.FirstOrDefault();

            if (status == null) return null;

            var timestamp = pod.Status.ContainerStatuses.FirstOrDefault()?.State.Running?.StartedAt;

            if (timestamp == null) return status.RestartCount.ToString();

            var ago = DateTime.UtcNow.Subtract(timestamp.Value);
            var agoStr = GetUserFriendlyTimestamp(ago);

            return $"{status.RestartCount} ({agoStr} ago)";
        }

        public static string? GetReadyStatus(this V1Pod pod)
        {
            return $"{pod.Status.ContainerStatuses?.Count(s => s.Ready) ?? 0}/{pod.Spec.Containers.Count}";
        }

        public static bool GetIsReady(this V1Pod pod)
        {
            return (pod.Status.ContainerStatuses?.Count(s => s.Ready) ?? 0) == pod.Spec.Containers.Count;
        }

        public static string? GetStatus(this V1Pod pod)
        {
            var state = pod.Status.ContainerStatuses?.FirstOrDefault()?.State;

            if (state == null)
            {
                return null;
            }

            if (state.Running != null)
            {
                return nameof(state.Running);
            }
            
            return state.Waiting?.Reason 
                ?? state.Terminated?.Reason;
        }

        public static string? GetAge(this V1Pod pod)
        {
            var timestamp = pod.CreationTimestamp() ?? DateTime.Now;
            
            var ago = DateTime.UtcNow.Subtract(timestamp);

            return GetUserFriendlyTimestamp(ago);
        }

        private static string GetUserFriendlyTimestamp(TimeSpan ago)
        {
            if (ago.TotalDays >= 2) return $"{ago.Days}d";
            if (ago.TotalDays >= 1) return $"{ago.Days}d {ago.Hours}h";
            if (ago.TotalHours >= 2) return $"{ago.Hours}h";
            if (ago.TotalHours >= 1) return $"{ago.Hours}h {ago.Minutes}m";
            if (ago.TotalMinutes >= 2) return $"{ago.Minutes}m";
            if (ago.TotalMinutes >= 1) return $"{ago.Minutes}m {ago.Seconds}s";

            return $"{ago.Seconds}s";
        }
    }
}
