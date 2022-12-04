<#
    .Description    Demo script
    .Name           Progress Pod Demo
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
Write-Information "Script started!"
Write-Debug "Starting reporting the progress..."
# report fake progress
for ($i = 1; $i -le 100; $i++ ) {
    Write-Progress -Activity "Demo" -Status "Working" -PercentComplete $i
    Start-Sleep -Milliseconds 100
}
Write-Information "Script finished!"