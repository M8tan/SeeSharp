Add-Type -AssemblyName system.windows.forms
$Source = "C:\Projects\SeeSharp"
$Res = [System.Text.StringBuilder]::new()
foreach($Item in (Get-ChildItem -Path $Source -Filter *.cs -File )){
    #Write-Host "$($Item.Name): $(Get-Content -Path $Item.PSPath -Raw)`r`n"
    try {
        $Content = Get-Content -Path $Item.PSPath -Raw -ErrorAction Stop
        $Res.Append("$($Item.Name): $($Content)")
    } catch {
        [void]([System.Windows.Forms.MessageBox]::Show("Encountered an error:`r`n$($_.exception.message)", "Error", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error))
        return
    }
}

try {
    $Res | Set-Clipboard -Confirm:$false -ErrorAction Stop
} catch {
    [void]([System.Windows.Forms.MessageBox]::Show("Encountered an error:`r`n$($_.exception.message)", "Error", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error))   
    return
}
