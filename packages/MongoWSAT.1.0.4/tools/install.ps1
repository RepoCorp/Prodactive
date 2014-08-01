param($installPath, $toolsPath, $package, $project)

$path = [System.IO.Path]
$readmefile = $path::Combine($path::GetDirectoryName($project.FileName), "MongoWSAT-README.md") #path to your important file here
$DTE.ItemOperations.OpenFile($readmefile) 
