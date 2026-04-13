# If you don't have access to powershell on your machine, please do not hesitate to fetch the docker image to do this exercice.
# The code below is an example on how to fetch the json from the endpoint.
# Please provide the proper code to have a display that matches the question.
$albums = Invoke-WebRequest -URI http://jsonplaceholder.typicode.com/albums | ConvertFrom-Json
$photos = Invoke-WebRequest -URI http://jsonplaceholder.typicode.com/photos | ConvertFrom-Json

$report = foreach ($album in $albums) {
    $photoCount = ($photos | Where-Object { $_.albumId -eq $album.id }).Count

    [PSCustomObject]@{
        AlbumId    = $album.id
        AlbumTitle = $album.title
        PhotoCount = $photoCount
    }
}

$report | Sort-Object -Property PhotoCount, AlbumId -Descending | Select-Object -First 5
