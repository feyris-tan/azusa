SELECT  album.catalog, 
		album.name
FROM dump_vgmdb_album_relatedalbum root
JOIN dump_vgmdb_albums album ON root.relatedalbumid = album.id
WHERE root.albumid = @albumid