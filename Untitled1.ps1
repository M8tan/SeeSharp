$Source = "C:\Projects\Private\SeeSharp"
$Res = [System.Text.StringBuilder]::new()
foreach($Item in (Get-ChildItem -Path $Source -Filter *.cs -File )){
    #Write-Host "$($Item.Name): $(Get-Content -Path $Item.PSPath -Raw)`r`n"
    $Res.Append("$($Item.Name): $(Get-Content -Path $Item.PSPath -Raw)")
}

$Res | Set-Clipboard -Confirm:$false