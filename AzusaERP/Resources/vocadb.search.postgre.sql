SELECT id, name, artiststring, disctype, releasedate, catalognumber
FROM dump_vocadb.albums
WHERE LOWER(name) LIKE LOWER(@query)
OR LOWER(artiststring) LIKE LOWER(@query)
OR LOWER(catalognumber) LIKE LOWER(@query)