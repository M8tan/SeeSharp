$Source = "C:\Projects\Private\SeeSharp"
foreach($Item in (Get-ChildItem -Path $Source -Filter *.cs -File )){
    Write-Host "$($Item.Name): $(Get-Content -Path $Item.PSPath -Raw)`r`n"

}