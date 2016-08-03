param([String]$key,[String]$version)

function setProjectVersion([String]$fileName, [String]$version) {
    $content = (Get-Content $fileName) -join "`n" | ConvertFrom-Json
    $content.version = $version
    $newContent = ConvertTo-Json -Depth 10 $content
    Set-Content $fileName $newContent
}

if ($version -ne "") {
	setProjectVersion ".\src\Folke.Identity.Elm\project.json" $version
	
	& dotnet restore

	cd .\src\Folke.Identity.Elm
	& dotnet pack -c Release
	$file = Get-Item "bin\Release\*.$version.nupkg"
	nuget push $file.FullName $key -Source https://api.nuget.org/v3/index.json
	cd ..\..
}