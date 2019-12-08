SELECT album.name
FROM dump_vocadb_albumtracks root
JOIN dump_vocadb_albums album ON root.albumid = album.id
WHERE lower(root.name) like lower(@query)