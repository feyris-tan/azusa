SELECT albums.catalog,
       albums.name
FROM dump_vgmdb_album_reprints root
JOIN dump_vgmdb_albums albums ON root.reprintid = albums.id
WHERE root.albumid = @albumid