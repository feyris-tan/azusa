SELECT albums.catalog,
       albums.name
FROM dump_vgmdb.album_reprints root
JOIN dump_vgmdb.albums albums ON root.reprintid = albums.id
WHERE root.albumid = @albumid