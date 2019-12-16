SELECT  album.catalog, 
		album.name
FROM dump_vgmdb.album_relatedalbum root
JOIN dump_vgmdb.albums album ON root.relatedalbumid = album.id
WHERE root.albumid = @albumid