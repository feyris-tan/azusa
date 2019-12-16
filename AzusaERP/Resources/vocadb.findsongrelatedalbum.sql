SELECT album.name
FROM dump_vocadb.albumtracks root
JOIN dump_vocadb.albums album ON root.albumid = album.id
WHERE lower(root.name) like lower(@query)