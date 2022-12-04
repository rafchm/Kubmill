<#
    .Description    Returns detailed pod information
    .Name           Describe Pod
    .ContextType    Pod
#>
param(
     [Parameter(Mandatory=$true)]
     [string]$context, 
     [Parameter(Mandatory=$true)]
     [string]$namespace,
     [Parameter(Mandatory=$true)]
     [string]$podname
)
kubectl config use-context $context
kubectl describe pod $podname -n $namespace
